using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Results;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTest", menuName = "TestScriptableObject", order = 0)]
    public class TestScriptableObject : ScriptableObject
    {
        public string title;
        public string description;
        public QuestionGroup[] groups;

        public List<ResultFieldModel> resultFields = new List<ResultFieldModel>();

        public void OnValidate()
        {
            if (resultFields.All(f => f.groupName != "Main"))
            {
                resultFields.Insert(0, new ResultFieldModel()
                {
                    groupName = "Main",
                    layerModels = new []
                    {
                        new ResultFieldLayerModel()
                        {
                            globalRightnessRangeMax = 1.1f,
                            globalRightnessRangeMin = 0.66f,
                            resultTitle = "Expert"
                        },
                        new ResultFieldLayerModel()
                        {
                            globalRightnessRangeMax = 0.66f,
                            globalRightnessRangeMin = 0.33f,
                            resultTitle = "Medium"
                        },
                        new ResultFieldLayerModel()
                        {
                            globalRightnessRangeMax = 0.33f,
                            globalRightnessRangeMin = -0.1f,
                            resultTitle = "Noob"
                        }
                    }
                });
            }

            foreach (QuestionGroup questionGroup in groups)
            {
                if (resultFields.All(f => f.groupName != questionGroup.name))
                {
                    resultFields.Add( new ResultFieldModel()
                    {
                        groupName = questionGroup.name,
                        layerModels = new []
                        {
                            new ResultFieldLayerModel()
                            {
                                globalRightnessRangeMax = 1.1f,
                                globalRightnessRangeMin = -0.1f,
                                resultTitle = questionGroup.name
                            },
                        }
                    });
                }
            }

            if (groups.Length > 2)
            {
                var pairs = new List<(QuestionGroup, QuestionGroup)>();

                for (int i = 0; i < groups.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        pairs.Add((groups.First(), groups.Last()));
                    }
                    pairs.Add((groups[i], groups[i + 1]));
                }

                foreach (var groupPair in pairs)
                {
                    if (resultFields.All(f => f.groupName != groupPair.Item1.name + groupPair.Item2.name))
                    {
                        resultFields.Add( new ResultFieldModel()
                        {
                            groupName = groupPair.Item1.name + groupPair.Item2.name,
                            layerModels = new []
                            {
                                new ResultFieldLayerModel()
                                {
                                    globalRightnessRangeMax = 1.1f,
                                    globalRightnessRangeMin = -0.1f,
                                    resultTitle = groupPair.Item1.name + groupPair.Item2.name
                                },
                            }
                        });
                    }
                }
            }

        }
    }
}