using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Results;
using UnityEngine;

namespace Tools
{
    public class TestResultCalculator
    {
        public static (float, string, string) CalculateResults(TestModel test)
        {
            var globalRightness = test.AllQuestions.Count(q => q.IsRightSelected) / (float) test.QuestionCount;
            
            var groupPoints = new Dictionary<QuestionGroupModel, Vector2>();
            var groupRightnessPoints = new Dictionary<QuestionGroupModel, Vector2>();

            var angle = 0f;
            var angleStep = 360f / test.Groups.Length;
            foreach (QuestionGroupModel group in test.Groups)
            {
                var rightness = group.Questions.Count(q=>q.IsRightSelected) / (float) group.Questions.Length;
                var point = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
                Debug.Log($"Create point for group {group.Name} {point.x},{point.y} rightness: {rightness}, angle: {angle}");
                groupPoints.Add(group, point);
                groupRightnessPoints.Add(group, point * rightness);
                angle += angleStep;
            }

            var centerPoint = new Vector2();

            foreach (KeyValuePair<QuestionGroupModel,Vector2> keyValuePair in groupRightnessPoints)
            {
                centerPoint += keyValuePair.Value;
            }

            centerPoint /= groupRightnessPoints.Count;
            Debug.Log($"Center point in {centerPoint.x},{centerPoint.y}");

            if (centerPoint.magnitude <= 0.25f)
            {
                // Is main field
                var mainFiled = test.ResultFields.First(f => f.groupName == "Main");

                return GetValuesFromLayers(mainFiled, globalRightness);
            }

            ResultFieldModel closestGroupField = null;
            float closestDistance = float.MaxValue;

            foreach (ResultFieldModel resultFieldModel in test.ResultFields.Where(f=>f.groupName != "Main"))
            {
                float distance;
                var groups = test.Groups.Where(g => resultFieldModel.groupName.Contains(g.Name)).ToList();
                if (!groups.Any())
                {
                    Debug.LogWarning($"Groups for field: {resultFieldModel.groupName} not found");
                    continue;
                }
                if (groups.Count == 1)
                {
                    distance = Vector2.Distance(centerPoint, groupPoints[groups[0]] * 0.5f);
                }
                else if (groups.Count == 2)
                {
                    var p0 = groupPoints[groups[0]];
                    var p1 = groupPoints[groups[1]];
                    distance = Vector2.Distance(centerPoint, p0 + (p1 - p0) / 2f);
                }
                else
                {
                    Debug.LogWarning($"Groups for field: {resultFieldModel.groupName} founded {groups.Count()} groups, it is strange");
                    continue;
                }

                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestGroupField = resultFieldModel;
                }
            }

            if (closestGroupField is null) throw new Exception("Field is not found");

            return GetValuesFromLayers(closestGroupField, globalRightness);
        }

        private static (float, string, string) GetValuesFromLayers(ResultFieldModel mainFiled, float globalRightness)
        {
            foreach (ResultFieldLayerModel layer in mainFiled.layerModels)
            {
                if (globalRightness > layer.globalRightnessRangeMin &&
                    globalRightness <= layer.globalRightnessRangeMax)
                {
                    return (globalRightness, layer.resultTitle, layer.resultDescription);
                }
            }

            throw new Exception("Field does not contains needed rightness");
        }
    }
}