namespace DatabaseRespository.Sql
{
    public interface ISqlRepository<I> : IDbRepository<SqlRepository<I>, I>
    {
    }
}
