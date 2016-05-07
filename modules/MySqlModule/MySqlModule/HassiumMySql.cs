using System;

using Hassium.Runtime;
using Hassium.Runtime.StandardLibrary;
using Hassium.Runtime.StandardLibrary.Types;

using MySql.Data.MySqlClient;

namespace MySqlModule
{
    public class HassiumMySql: HassiumObject
    {
        public new MySqlConnection Value { get; private set; }
        public HassiumMySql()
        {
        }

        private HassiumMySql _new(VirtualMachine vm, HassiumObject[] args)
        {
            HassiumMySql hassiumMySql = new HassiumMySql();

            hassiumMySql.Value = new MySqlConnection(string.Format("Server={0};Database{1};User ID={2};Password={3};Pooling=false", args[0].ToString(vm), args[1].ToString(vm), args[2].ToString(vm), args[3].ToString(vm)));
            hassiumMySql.Attributes.Add("close",    new HassiumFunction(hassiumMySql.close, 0));
            hassiumMySql.Attributes.Add("open",     new HassiumFunction(hassiumMySql.open, 0));
            hassiumMySql.Attributes.Add("query",    new HassiumFunction(hassiumMySql.query, 3));
            hassiumMySql.Attributes.Add("select",   new HassiumFunction(hassiumMySql.select, -1));

            return hassiumMySql;
        }

        public HassiumNull close(VirtualMachine vm, HassiumObject[] args)
        {
            Value.Close();
            return HassiumObject.Null;
        }
        public HassiumNull open(VirtualMachine vm, HassiumObject[] args)
        {
            Value.Open();
            return HassiumObject.Null;
        }
        public HassiumNull query(VirtualMachine vm, HassiumObject[] args)
        {
            MySqlCommand command = new MySqlCommand(null, Value);
            command.CommandText = "INSERT INTO " + args[0].ToString(vm) + "(";
            HassiumObject[] vals = HassiumList.Create(args[1]).Value.ToArray();
            command.CommandText += vals[0].ToString(vm);
            for (int x = 1; x < vals.Length; x++)
                command.CommandText += ", " + vals[x].ToString(vm);
            command.CommandText += ") VALUES (";
            command.CommandText += "@" + vals[0].ToString(vm);
            for (int x = 1; x < vals.Length; x++)
                command.CommandText += ", @" + vals[x].ToString(vm);
            command.CommandText += ")";

            HassiumObject[] lits = HassiumList.Create(args[2]).Value.ToArray();
            for (int x = 0; x < lits.Length; x++) {
                MySqlParameter param = new MySqlParameter("@" + vals[x].ToString(vm), lits[x].ToString(vm));
                param.Value = lits[x].ToString(vm);
                command.Parameters.Add(param);
            }
           
            command.Prepare();
            command.ExecuteNonQuery();
            return HassiumObject.Null;
        }
        private HassiumMySqlDataReader select(VirtualMachine vm, HassiumObject[] args)
        {
            MySqlCommand command = new MySqlCommand(args[0].ToString(vm), Value);
            for (int x = 1; x < args.Length; x++)
                command.Parameters.AddWithValue(HassiumList.Create(args[x]).Value[0].ToString(vm), HassiumList.Create(args[x]).Value[1].ToString(vm));
            command.Prepare();

            return new HassiumMySqlDataReader(command.ExecuteReader());
        }
    }
}

