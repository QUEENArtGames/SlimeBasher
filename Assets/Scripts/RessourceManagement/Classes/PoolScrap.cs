public class PoolScrap
{
    private ScrapType scrapType;
    private int subType;

    public PoolScrap(ScrapType scrapType, int subType)
    {
        this.scrapType = scrapType;
        this.subType = subType;
    }

    public ScrapType ScrapType
    {
        get
        {
            return scrapType;
        }
    }

    public int SubType
    {
        get
        {
            return subType;
        }      
    }
}