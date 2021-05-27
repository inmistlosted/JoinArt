using diplom.api.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class DataAccessAdapter : IDataAccessAdapter
    {
        public IPaintingAdapter PaintingAdapter { get; }
        public IUserAdapter UserAdapter { get; }
        public IGenreAdapter GenreAdapter { get; }
        public IAlbumAdapter AlbumAdapter { get; }
        public IAuctionAdapter AuctionAdapter { get; }
        public IOrderAdapter OrderAdapter { get; }

        public DataAccessAdapter(DataAccessSettings dataAccessSettings)
        {
            if (dataAccessSettings == null)
            {
                throw new ArgumentNullException(nameof(dataAccessSettings));
            }

            ICommandAdapter commandAdapter = new PgSqlCommandAdapter(dataAccessSettings);

            this.PaintingAdapter = new PaintingAdapter(commandAdapter);
            this.UserAdapter = new UserAdapter(commandAdapter);
            this.GenreAdapter = new GenreAdapter(commandAdapter);
            this.AlbumAdapter = new AlbumAdapter(commandAdapter);
            this.AuctionAdapter = new AuctionAdapter(commandAdapter);
            this.OrderAdapter = new OrderAdapter(commandAdapter);
        }
    }
}
