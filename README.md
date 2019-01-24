# AdoContextUtility

### What's this?
This is just a humble attempt to bring easy usage of Ado.Net store procedure for those who not want to go deep into documentation to do a easy task. If you need a broader connexion control to your database I recommend you to use ADO.Net directly.

_Any observation or contribution will be welcome._
***

### AdoContextUtility Usage Example
***
You have a SQL Store Procedure with name SP_RetrieveData with parameters Param1, Param2 and return information field Field1, Field2 and you declare a class modeling this store procedure as follow:

    internal class SP_RetrieveData
    {
        public string Param1 { get; set; }
        public int Param2 { get; set; }
        public string Field1 { get; set; }
        public double Field2 { get; set; }
    }

    void UsingAdoContext(){
            SP_RetrieveData sp_RetrieveData = new SP_RetrieveData();
            StoreProcedureInfo storeProcedureInfo =
                new StoreProcedureInfo(nameof(SP_RetrieveData))
                .AddParameter(() => sp_RetrieveData.Param1).Setting(null, null, ParameterDirection.Input)
                .AddParameter(() => sp_RetrieveData.Param2 ).Setting(null, null, ParameterDirection.Input)
                .AddParameter(() => sp_RetrieveData.Field1).Setting(null, null, ParameterDirection.ReturnValue)
                .AddParameter(() => sp_RetrieveData.Field2).Setting(null, null, ParameterDirection.ReturnValue)
                ;
            IAdoContext adoContext = new AdoContext(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
            (dynamic result, IList ls) = adoContext.Execute(storeProcedureInfo);

            for(int i = 0; i < ls.Count; ++i)
            {
                dynamic entity = ls[i];
                string Field1 = entity.Field1;
                double Field2 = entity.Field2;
                DoStuff(Field1, Field2);
            }
        }
        
***
### Mapper Usage Example with Attributes
***
    public class SP_RetrieveData
    {
        [AdoEntityDescription(direction = System.Data.ParameterDirection.Input)]
        public string Input1 { get; set; }

        [AdoEntityDescription(direction = System.Data.ParameterDirection.ReturnValue)]
        public string Result2 { get; set; }

        [AdoEntityDescription(direction = System.Data.ParameterDirection.ReturnValue)]
        public string Result3 { get; set; }
    }
  
    public Entity
    {
        [Mapper(FieldName = "Result1")]
        public string NameResult1 { get; set; }
        public string Others { get; set; }
        [Mapper(FieldName = "Result2")]
        public string NameResult2 { get; set; }
    }
    
    void Using AdoContext()
    {
            SP_RetrieveData sp_RetrieveData = new SP_RetrieveData();
            IAdoContext adoContext = new AdoContext(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);

            (dynamic result, IList ls) = adoContext.Execute(sp_RetrieveData);

            List<Entity> entities = Map.Instance.MapData<SP_RetrieveData, Entity>((result, ls));

            foreach (var entity in entities)
            {
                DoStuff(entity);
            }
    }

