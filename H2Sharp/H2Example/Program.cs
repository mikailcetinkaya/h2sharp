using System;
using System.Data;
using System.Data.H2;
using System.Diagnostics;

namespace H2Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var connection = new H2Connection("jdbc:h2:mem:test");
            connection.Open();
            new H2Command("create table list (item integer primary key, description varchar(256), value integer)", connection).ExecuteNonQuery();
            new H2Command("insert into list values (1, 'First Item', 10)", connection).ExecuteNonQuery();
            new H2Command("insert into list values (2, 'Second Item', 11)", connection).ExecuteNonQuery();

            var table = new DataTable("test")
            {
                CaseSensitive = false
            };
            var adapter = new H2DataAdapter("select item, description, value from list order by item", connection);
            new H2CommandBuilder(adapter);
            adapter.Fill(table);

            table.Rows[1][1] = "First item modified";
            table.Rows[1][2] = 12;
            table.Rows.Add(new object[] { 3, "Third", 15 });
            adapter.Update(table);

            var table2 = new DataTable("test")
            {
                CaseSensitive = false
            };
            adapter.Fill(table2);

            var x1 = table.ToXml();
            var x2 = table2.ToXml();
            Debug.Assert(x1.Equals(x2));

            var count = new H2Command("select count(*) from list", connection).ExecuteScalar();
            Debug.Assert(((long)count).Equals(3));

            var one = new H2Command("select 1 from dual", connection).ExecuteScalar();
            Debug.Assert(((int)one).Equals(1));

            var a = new H2Command("select 'a' from dual", connection).ExecuteScalar();
            Debug.Assert(((string)a).Equals("a"));

            var p = new H2Command("select /*fff*/-- ddd\r\n 'a' from dual where 1=:p0 and 2=2", connection);
            java.lang.Integer iq = java.lang.Integer.decode("1");
            p.Parameters.Add(new H2Parameter("p0",iq));
            var aa = p.ExecuteScalar();
            Debug.Assert(((string)aa).Equals("a"));
            var pp = new H2Command( @"--CREATE SCHEMA ocn_btc
                --    AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_cfg
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_clr
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_iss
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_acq
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_trash
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_cat
                --AUTHORIZATION postgres;

                            /*
                            CREATE OR REPLACE FUNCTION OCN_CFG.FN_GET_LANGUAGE_TEXT (
                               INPUT      IN VARCHAR,
                               LANGUAGE   IN VARCHAR)
                               RETURNS VARCHAR AS $L_LANGUAGE_TEXT$
                            DECLARE
                               L_LANGUAGE        VARCHAR := LANGUAGE;
                               L_LANGUAGE_TEXT   VARCHAR ;
                               L_DEFAULT_LANG    VARCHAR := 'en-US';
                            BEGIN
                               IF    INPUT IS NULL
                                  OR LENGTH (TRIM (INPUT)) = 0
                                  OR L_LANGUAGE IS NULL
                                  OR LENGTH (TRIM (L_LANGUAGE)) = 0
                               THEN
                                  RETURN INPUT;
                               END IF;

                               IF POSITION (L_LANGUAGE || ':=' IN INPUT) = 0
                               THEN
                                  L_LANGUAGE := UPPER(SUBSTRING(L_LANGUAGE FROM 1 FOR 2));
                                  IF POSITION (L_LANGUAGE|| ':=' IN INPUT) = 0
                                  THEN
                                      L_LANGUAGE := L_DEFAULT_LANG;
                                      IF POSITION (L_LANGUAGE || ':=' IN INPUT) = 0
                                      THEN
                                        L_LANGUAGE := UPPER(SUBSTRING(L_LANGUAGE FROM 1 FOR 2));
                                        IF POSITION (L_LANGUAGE|| ':=' IN INPUT) = 0
                                        THEN
                                             RETURN INPUT;
                                        END IF;
                                      END IF;
                                  END IF;
                               END IF;

                               L_LANGUAGE_TEXT :=
                                  SUBSTRING (INPUT FROM
                                          POSITION (L_LANGUAGE || ':=' IN INPUT) + LENGTH (l_language) + 2);

                               IF POSITION (':='  IN L_LANGUAGE_TEXT) > 0
                               THEN
                                  L_LANGUAGE_TEXT :=
                                     SUBSTRING (L_LANGUAGE_TEXT FROM 1 FOR POSITION (';;' IN L_LANGUAGE_TEXT) - 1);
                               END IF;

                               RETURN L_LANGUAGE_TEXT;
                            EXCEPTION
                               WHEN OTHERS
                               THEN
                                  raise;
                            END;
                            $L_LANGUAGE_TEXT$ LANGUAGE plpgsql;
                            */
                            ",connection);
            var aba = pp.ExecuteScalar();

            SimpleTest(connection, "int", 10, 11, 12);
            SimpleTest(connection, "bigint", (long)10, (long)11, (long)12);
            SimpleTest(connection, "smallint", (short)10, (short)11, (short)12);
            SimpleTest(connection, "tinyint", (byte)10, (byte)11, (byte)12);
            SimpleTest(connection, "double", 10d, 11d, 12d);
            SimpleTest(connection, "float", 10d, 11d, 12d); // double is float !
            SimpleTest(connection, "varchar(255)", "10", "11", "12");
            SimpleTest(connection, "timestamp", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            SimpleTest(connection, "date", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            //var x1 = table.ToXml();
            Console.WriteLine("Yuppiii , TEST OK.");
        }

        /// <summary>
        /// - Creates a table with one primary key
        /// - inserts a value
        /// - loads the table to a DataTable and checks it has the expected value
        /// - inserts another value, reloads and checks it's there
        /// - updates the previously inserted value, reloads and checks it has the expected updated value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="typeStr"></param>
        /// <param name="originalValue"></param>
        /// <param name="updatedValue"></param>
        /// <param name="insertedValue"></param>
        static void SimpleTest<T>(H2Connection connection, String typeStr, T originalValue, T updatedValue, T insertedValue)
        {
            var tn = "simple_test_" + typeStr.Replace('(', '_').Replace(')', '_');
            new H2Command("create table " + tn + " (value " + typeStr + " primary key)", connection).ExecuteNonQuery();
            dynamic val = originalValue;
            var originalValueStr =
                (originalValue is string) ? "'" + originalValue + "'" :
                (originalValue is DateTime) ? "parsedatetime('" + val.ToString("dd/MM/yyyy hh:mm:ss") +  "', 'dd/MM/yyyy hh:mm:ss')" :
                    originalValue.ToString();
            new H2Command("insert into " + tn + " values (convert(" + originalValueStr + ", " + typeStr + "))", connection).ExecuteNonQuery();

            var adapter = new H2DataAdapter("select * from " + tn + " order by value", connection);
            new H2CommandBuilder(adapter);
            var table = new DataTable(tn)
            {
                CaseSensitive = false
            };
            adapter.Fill(table);

            Debug.Assert(table.Rows.Count.Equals(1));
            CheckVal(table.Rows[0][0], originalValue);

            table.Rows.Add(new object[] { insertedValue });
            adapter.Update(table);

            Reload(table, adapter);
            Debug.Assert(table.Rows.Count.Equals(2));
            CheckVal(table.Rows[1][0], insertedValue);

            table.Rows[1][0] = updatedValue;
            adapter.Update(table);

            Reload(table, adapter);
            Debug.Assert(table.Rows.Count.Equals(2));
            CheckVal(table.Rows[1][0], updatedValue);

        }
        static void Reload(DataTable table, H2DataAdapter adapter)
        {
            table.Clear();
            table.AcceptChanges();
            adapter.Fill(table);
        }
        static void CheckVal<T>(object value, T expectedVal)
        {
            Debug.Assert(value is T);
            var tvalue = (T)value;
            Debug.Assert(expectedVal.Equals(tvalue));
        }
    }

}
