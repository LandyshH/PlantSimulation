using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public static class GrowthRateCalculator
    {
        public static float CalculateGrowthRate(EnvironmentSettings environment)
        {
            float growthRate = 0f;

            if (environment.CarbonDioxide == CarbonDioxide.Excess)
            {
                return growthRate;
            }

            growthRate += CalculatePhotosynthesisRate(environment);
            growthRate += CalculateWaterRate(environment);
            growthRate += CalculateOxygenRate(environment);
            growthRate += CalculateMineralsRate(environment);
            growthRate += CalculateTemperatureRate(environment);
            var carbonDioxide = CalculateCarbonDioxideRate(environment);
            growthRate += carbonDioxide;

            return growthRate * 0.1f;
        }

        private static float CalculatePhotosynthesisRate(EnvironmentSettings environment)
        {
            switch (environment.Light)
            {
                case LightColor.Darkness:
                    return 0.5f;
                case LightColor.Sun:
                    return 1f;
                case LightColor.Red:
                    return 1.5f;
                case LightColor.Blue:
                    return 0.5f;
                default: return 1f;
            }
        }

        private static float CalculateWaterRate(EnvironmentSettings environment)
        {
            switch (environment.Water)
            {
                case Water.Lack:
                    return 0.01f;
                case Water.Optimal:
                    return 1f;
                case Water.Excess:
                    return 0.3f;
                default: return 1f;
            }
        }

        private static float CalculateOxygenRate(EnvironmentSettings environment)
        {
            switch (environment.Oxygen)
            {
                case Oxygen.Lack:
                    return 0.01f;
                case Oxygen.Optimal:
                    return 1f;
                case Oxygen.Excess:
                    return 0.5f;
                default: return 1f;
            }
        }

        private static float CalculateCarbonDioxideRate(EnvironmentSettings environment)
        {
            switch (environment.CarbonDioxide)
            {
                case CarbonDioxide.Lack:
                    return 0.5f;
                case CarbonDioxide.Optimal:
                    return 1f;
                case CarbonDioxide.Excess:
                    return 0f;
                default: return 1f;
            }
        }

        private static float CalculateMineralsRate(EnvironmentSettings environment)
        {
            switch (environment.Minerals)
            {
                case Minerals.Lack:
                    return 0.5f;
                case Minerals.Optimal:
                    return 1f;
                case Minerals.Excess:
                    return 0.5f;
                default: return 1f;
            }
        }

        private static float CalculateTemperatureRate(EnvironmentSettings environment)
        {
            switch (environment.Temperature)
            {
                case Temperature.Min:
                    return 0.3f;
                case Temperature.Optimal:
                    return 1f;
                case Temperature.Max:
                    return 0f;
                default: return 1f;
            }
        }
    }
}
