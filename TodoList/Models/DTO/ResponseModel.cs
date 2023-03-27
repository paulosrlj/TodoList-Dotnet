namespace TodoList.Models.DTO
{
    public class ResponseModel
    {

        public object Data { get; set; }

        public ResponseModel(object data)
        {
            Data = data;
        }

        public ResponseModel(IEnumerable<object> errors)
        {
            var errorsDict = new Dictionary<string, IEnumerable<object>>() { { "errors", errors } };
            Data = errorsDict;
        }
    }
}
