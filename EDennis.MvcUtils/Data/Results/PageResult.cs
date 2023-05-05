namespace EDennis.MvcUtils
{
    public class PageResult<TEntity>
    {
        /// <summary>
        /// The page of data
        /// </summary>
        public List<TEntity> Data { get; set; }
        
        /// <summary>
        /// The count of records across all pages
        /// </summary>
        public int CountAcrossPages { get; set; }
    }

    /// The result returned from a Dynamic Linq query.  This
    /// version of DynamicLinqResult provides a dynamic response.
    /// The class is used when the Select extension method is
    /// used to return only a subset of properties.  
    /// See <see cref="DynamicLinqResult{TEntity}"/>
    public class PageResult
    {
        /// <summary>
        /// The page of data
        /// </summary>
        public List<object> Data { get; set; }

        /// <summary>
        /// The count of records across all pages
        /// </summary>
        public int CountAcrossPages { get; set; }
    }

}
