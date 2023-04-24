namespace UserManager.WebAPI.Attribute;

//只能在方法中使用此属性
[AttributeUsage(AttributeTargets.Method)]
public class UnitOfWorkAttribute : System.Attribute
{
    public  Type[] DbContextTypes { get; set; }

    public UnitOfWorkAttribute(params Type[] dbContextTypes)
    {
        this.DbContextTypes = dbContextTypes;
    }
}