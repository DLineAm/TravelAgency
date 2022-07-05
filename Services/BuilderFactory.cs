using System;

namespace TravelAgency.Services
{
    /// <summary>
    /// Класс фабрики, создающий экземпляр строителя
    /// </summary>
    public static class BuilderFactory
    {
        /// <summary>
        /// Создает конкретный экземпляр строителя
        /// </summary>
        /// <typeparam name="T">Тип строителя (IBuilder) </typeparam>
        /// <returns>Конркетный экземпляр IBuilder</returns>
        public static T NewBuilder<T>() where T : IBuilder
        {
            var instance = Activator.CreateInstance(typeof(T));
            return (T)instance;
        }
    }
}