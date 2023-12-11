using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Helpers
{
    public static class ModelMapper
    {

        public static HoroscopeData MappedHoroscopeDataFromExternal(HoroscopeDataExternal horoscopeDataExternal)
        {
            PredictionData predictionData = new PredictionData
            {
                PersonalLife = horoscopeDataExternal.Prediction.PersonalLife,
                Profession = horoscopeDataExternal.Prediction.Profession,
                Emotions = horoscopeDataExternal.Prediction.Emotions,
                Luck = horoscopeDataExternal.Prediction.Luck,
                Health = horoscopeDataExternal.Prediction.Health,
                Travel = horoscopeDataExternal.Prediction.Travel,

            };
            HoroscopeData horoscopeData = new HoroscopeData
            {
                SunSign = horoscopeDataExternal.SunSign,
                PredictionDate = horoscopeDataExternal.PredictionDate,
                Prediction = predictionData
            };
            return horoscopeData;
        }

        public static HoroscopeDataExternal MappedHoroscopeDataFromInternal(HoroscopeData cachedHoroscopeData)
        {
            var horoscopeData = new HoroscopeDataExternal();
            var PredictionData = new PredictionDataExternal()
            {
                PersonalLife = cachedHoroscopeData.Prediction.PersonalLife,
                Profession = cachedHoroscopeData.Prediction.Profession,
                Luck = cachedHoroscopeData.Prediction.Luck,
                Emotions = cachedHoroscopeData.Prediction.Emotions,
                Health = cachedHoroscopeData.Prediction.Health,
                Travel = cachedHoroscopeData.Prediction.Travel
            };


            horoscopeData.SunSign = cachedHoroscopeData.SunSign;
            horoscopeData.PredictionDate = cachedHoroscopeData.PredictionDate;
            horoscopeData.Prediction = PredictionData;
            return horoscopeData;
        }
    }
}
