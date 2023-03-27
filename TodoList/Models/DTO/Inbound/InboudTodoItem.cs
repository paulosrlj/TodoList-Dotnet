namespace TodoList.Models.DTO.Inbound
{
    public class InboundTodoItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
    }
}