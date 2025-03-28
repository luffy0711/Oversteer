using UnityEngine;
using System.Collections.Generic;

public class GachaManager : MonoBehaviour
{
    [System.Serializable]
    public class CarPool
    {
        public List<CarData> cars;
        public List<float> weights;
    }

    public CarPool threeStarPool;
    public CarPool fourStarPool;
    public CarPool fiveStarPool;

    public int pullsSinceLastFourStar = 0;
    public int pullsSinceLastFiveStar = 0;

    private const float fiveStarRate = 0.05f;  // 5% chance
    private const float fourStarRate = 0.30f;  // 30% chance

    private const int fourStarPity = 10;  // Guaranteed 4★ at 10 pulls
    private const int fiveStarPity = 50;  // Guaranteed 5★ at 50 pulls

    public void PerformGachaPull()
    {
        pullsSinceLastFourStar++;
        pullsSinceLastFiveStar++;

        float roll = Random.value;

        CarData wonCar = null;

        if (pullsSinceLastFiveStar >= fiveStarPity || roll <= fiveStarRate)
        {
            wonCar = GetWeightedRandomCar(fiveStarPool);
            pullsSinceLastFiveStar = 0; // Reset pity
        }
        else if (pullsSinceLastFourStar >= fourStarPity || roll <= fourStarRate)
        {
            wonCar = GetWeightedRandomCar(fourStarPool);
            pullsSinceLastFourStar = 0;
        }
        else
        {
            wonCar = GetWeightedRandomCar(threeStarPool);
        }

        UnlockCar(wonCar);
    }

    private CarData GetWeightedRandomCar(CarPool pool)
    {
        if (pool.cars.Count == 0) return null;

        float totalWeight = 0;
        foreach (float weight in pool.weights) totalWeight += weight;

        float randomPoint = Random.Range(0, totalWeight);
        float currentWeight = 0;

        for (int i = 0; i < pool.cars.Count; i++)
        {
            currentWeight += pool.weights[i];
            if (randomPoint <= currentWeight)
            {
                return pool.cars[i];
            }
        }

        return pool.cars[pool.cars.Count - 1]; // Fallback
    }

    private void UnlockCar(CarData car)
    {
        Debug.Log($"Unlocked: {car.carName}");
        // Add logic to store unlocked cars
    }
}
