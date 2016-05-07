using System;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary;
using Hassium.Runtime.StandardLibrary.Types;

using MySql.Data.MySqlClient;

namespace MySqlModule
{
    public class HassiumMySqlDataReader: HassiumObject
    {
        public new MySqlDataReader Value { get; private set; }
        public HassiumMySqlDataReader(MySqlDataReader reader)
        {
            Value = reader;
            Attributes.Add("close",         new HassiumFunction(close, 0));
            Attributes.Add("dispose",       new HassiumFunction(dispose, 0));
            Attributes.Add("get",           new HassiumFunction(get, 1));
            Attributes.Add("getString",     new HassiumFunction(getString, 1));
            Attributes.Add("nextResult",    new HassiumFunction(nextResult, 0));
            Attributes.Add("read",          new HassiumFunction(read, 0));
        }

        private HassiumNull close(VirtualMachine vm, HassiumObject[] args)
        {
            Value.Close();
            return HassiumObject.Null;
        }
        private HassiumNull dispose(VirtualMachine vm, HassiumObject[] args)
        {
            Value.Dispose();
            return HassiumObject.Null;
        }
        private HassiumString get(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumString(Value[HassiumString.Create(args[0]).Value].ToString());
        }
        private HassiumString getString(VirtualMachine vm, HassiumObject[] args)
        {
            if (args[0] is HassiumInt)
                return new HassiumString(Value.GetString((int)HassiumInt.Create(args[0]).Value));
            else if (args[0] is HassiumString)
                return new HassiumString(Value.GetString(HassiumString.Create(args[0]).Value));
            else
                return new HassiumString(Value.GetString(args[0].ToString(vm)));
        }
        private HassiumBool nextResult(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumBool(Value.NextResult());
        }
        private HassiumBool read(VirtualMachine vm, HassiumObject[] args)
        {
            return new HassiumBool(Value.Read());
        }
    }
}

