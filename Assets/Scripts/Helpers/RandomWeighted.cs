using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public static class RandomWeighted
{
    public static T Roll<T>(Dictionary<int, T> table) where T : class
    {
        int sumOfWeights = 0;
        foreach (var weightValuePair in table)
        {
            int weight = weightValuePair.Key;
            sumOfWeights += weight;
        }
        int x = UnityEngine.Random.Range(0, sumOfWeights);
        return LookupValue(table, x);
    }
    
    private static T LookupValue<T>(Dictionary<int, T> table, int x) where T : class
    {
        // assume 0 ≤ x < sum_of_weights
        var cumulative_weight = 0;
        foreach (var weightValuePair in table)
        {
            int weight = weightValuePair.Key;
            T value = weightValuePair.Value;
            cumulative_weight += weight;
            if (x < cumulative_weight)
            {
                return value;
            }
        }
        return null;
    }
    
    public static Entity Roll(Dictionary<int, Entity> table)
    {
        int sumOfWeights = 0;
        foreach (var weightValuePair in table)
        {
            int weight = weightValuePair.Key;
            sumOfWeights += weight;
        }
        int x = UnityEngine.Random.Range(0, sumOfWeights);
        return LookupValue(table, x);
    }
    
    private static Entity LookupValue(Dictionary<int, Entity> table, int x)
    {
        // assume 0 ≤ x < sum_of_weights
        var cumulative_weight = 0;
        foreach (var weightValuePair in table)
        {
            int weight = weightValuePair.Key;
            Entity value = weightValuePair.Value;
            cumulative_weight += weight;
            if (x < cumulative_weight)
            {
                return value;
            }
        }
        return Entity.Null;
    }
    
}
