namespace diplom.api.DataAccessLayer
{
    public interface IDataAccessAdapter
    {
        IPaintingAdapter PaintingAdapter { get; }
        IUserAdapter UserAdapter { get; }
        IGenreAdapter GenreAdapter { get; }
        IAlbumAdapter AlbumAdapter { get; }
        IAuctionAdapter AuctionAdapter { get; }
        IOrderAdapter OrderAdapter { get; }
    }
}
