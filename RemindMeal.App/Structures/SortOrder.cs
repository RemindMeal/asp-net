namespace RemindMeal.Structures
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public static class SortOrderExtensions
    {
        public static SortOrder Invert(this SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    return SortOrder.Descending;
                case SortOrder.Descending:
                    return SortOrder.Ascending;
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, null);
            }
        }

        public static SortOrder ToSortOrder(this string sortOrderString)
        {
            return sortOrderString == "desc" ? SortOrder.Descending : SortOrder.Ascending;
        }

        public static string ConvertToString(this SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "asc";
                case SortOrder.Descending:
                    return "desc";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null);
            }
        }
    }
}