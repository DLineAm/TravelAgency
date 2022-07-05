//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using TravelAgency.Models;

//namespace TravelAgency.Services
//{
//    public class CommandBuilder : IBuilder
//    {
//        private readonly SqlCommand _command;
//        public CommandBuilder()
//        {
//            _command = new SqlCommand();
//        }

//        public CommandBuilder AddParameter(string parameter, object value)
//        {
//            _command.Parameters.AddWithValue(parameter, value);
//            return this;
//        }

//        public CommandBuilder SetConnection(SqlConnection connection)
//        {
//            _command.Connection = connection;
//            return this;
//        }

//        /// <summary>
//        /// Задает текст команды без использования генератора команд
//        /// </summary>
//        /// <param name="command">Текст команды</param>
//        /// <returns>Строитель SQL-команд</returns>
//        public CommandBuilder SetCommandText(string command)
//        {
//            _command.CommandText = command;
//            return this;
//        }

//        public IBuilder NewBuilder()
//        {
//            return new CommandBuilder();
//        }

//        public object Build()
//        {
//            return _command;
//        }

//        /// <summary>
//        /// Генерирует UPDATE и INSERT Sql скрипт
//        /// </summary>
//        /// <typeparam name="T">Тип записи</typeparam>
//        /// <param name="value">Объект (запись в базе данных)</param>
//        /// <param name="commandType">Тип скрипта (INSERT / UPDATE)</param>
//        /// <exception cref="InvalidOperationException">Исключение при выборе неправильного типа команды</exception>
//        private void GenerateAttributesString<T>(T value, CommandType commandType) where T : Model
//        {
//            var valueType = value?.GetType();
//            if (valueType is null) return;
//            var attrs = valueType.GetCustomAttributes(false);
//            var avaibleProps = attrs.FirstOrDefault(p => p is AvailablePropertiesAttribute) as AvailablePropertiesAttribute;
//            //Get list of properties
//            var properties = valueType.GetProperties();
//            var arrayOfNames = properties.Select(p => p.Name).ToArray();
//            var arrayOfValues = properties.Select(p => p.GetValue(value)?.ToString()).ToArray();
//            var listOfParameters = new List<string>();
//            string updateStringOfProperties = null;
//            var stringOfProperties = "";
//            //union list of properties to a string
//            stringOfProperties += string.Join("", properties.Select(p =>
//                !(p.Name == "Id" && commandType == CommandType.Insert) && !(avaibleProps != null && properties.ToList().IndexOf(p) >= avaibleProps.Count)
//                    ? properties.ToList().IndexOf(p) != 0
//                        ? ", " + p.Name
//                        : p.Name
//                    : null));

//            //generate list of attributes
//            string stringOfParameters = null;
//            for (var i = 0; avaibleProps != null ? i < avaibleProps.Count : i < properties.Length; i++)
//            {
//                listOfParameters.Add($"@{i}");
//                if (arrayOfValues[i] == "0")
//                    continue;
//                stringOfParameters += i != 0
//                    ? ", " + $"@{i}"
//                    : $"@{i}";
//                updateStringOfProperties += i != 0
//                    ? !(commandType == CommandType.Update && i == properties.Length - 1)
//                        ? ", " + arrayOfNames[i] + " = " + listOfParameters[i]
//                        : null
//                    : arrayOfNames[i] + " = " + listOfParameters[i];
//            }

//            var arrayOfAttributes = listOfParameters.ToArray();

//            var resultString = commandType switch
//            {
//                CommandType.Insert =>
//                    $"INSERT INTO {value.GetType().Name} ({stringOfProperties}) output INSERTED.Id VALUES ({stringOfParameters})",
//                CommandType.Update => $"UPDATE {value.GetType().Name} " + $"SET {updateStringOfProperties}" +
//                                      $" WHERE Id = {value?.Id}",
//                _ => throw new InvalidOperationException("CommandType set incorrectly. Choose another CommandType")
//            };

//            SetCommandText(resultString);

//            for (var i = 0; avaibleProps != null ? i < avaibleProps.Count : i < properties.Length; i++)
//            {
//                AddParameter(arrayOfAttributes[i],
//                    DateTime.TryParse(arrayOfValues[i], out var time) ? (object)time : arrayOfValues[i]);
//            }
//        }


//        /// <summary>
//        /// Задает текст команды с использованием генератора команд
//        /// </summary>
//        /// <typeparam name="T">Тип объекта</typeparam>
//        /// <param name="commandType">Тип команды</param>
//        /// <param name="value">Объект (запись в базе данных)</param>
//        /// <returns>Строитель SQL-команд</returns>
//        //public CommandBuilder SetCommandText<T>(CommandType commandType, T value = null) where T : Model
//        //{
//        //    switch (commandType)
//        //    {
//        //        case CommandType.Delete:
//        //            SetCommandText($"DELETE FROM {value.GetType().Name} WHERE Id = {value.Id}");
//        //            break;
//        //        default:
//        //            GenerateAttributesString(value, commandType);
//        //            break;
//        //    }
//        //    return this;
//        //}

//        public CommandBuilder SetTransaction(SqlTransaction transaction)
//        {
//            _command.Transaction = transaction;
//            return this;
//        }

//        private void GetValue<T>() where T : Model
//        {
//            var value = (Model)Activator.CreateInstance(typeof(T));
//            _command.CommandText = value switch
//            {
//                Vouchers voucher =>
//                    "select v.Id, v.StartDate, v.EndDate, v.Duration, v.ClientId, v.HotelId, v.StuffId," +
//                    " v.AdditService1Id, v.AdditService2Id, v.AdditService3Id, v.RestTypeId, v.PaymentStatus," +
//                    " v.BookingStatus, c.Name, c.Surname, c.Patronymic, h.Name, s.Name, s.Surname," +
//                    " s.Patronymic, a1.Name, a2.Name, a3.Name, r.Name" + " from Vouchers as v" +
//                    " inner join Clients as c on" + " v.ClientId = c.Id" + " inner join Hotels as h on" +
//                    " v.HotelId = h.Id" + " inner join Stuff as s on" + " v.StuffId = s.Id" +
//                    " inner join AdditionalServices as a1 on" + " v.AdditService1Id = a1.Id" +
//                    " inner join AdditionalServices as a2 on" + " v.AdditService2Id = a2.Id" +
//                    " inner join AdditionalServices as a3 on" + " v.AdditService3Id = a3.Id" +
//                    " inner join RestTypes as r on" + " v.RestTypeId = r.Id",
//                _ => $"SELECT * FROM {typeof(T).Name}"
//            };
//        }

//        public enum CommandType
//        {
//            Delete, Insert, Update
//        }
//    }
//}