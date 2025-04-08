namespace BusinessLogic.Attributes
{
    public class ServiceAttribute : Attribute
    {
        public string Description { get; }
        public int Order { get; }

        public ServiceAttribute(string description, int order)
        {
            Description = description;
            Order = order;
        }
    }
}
