namespace ArffFileProcesser
{
    public interface IFileProcesser<out TFileProcessed>
    {
        ArffModel Process(object input);
    }
}