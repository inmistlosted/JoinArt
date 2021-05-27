import React, {Component} from 'react';
import '../componentsStyles/UserPage.css';
import UserService from '../services/UserService';
import {Loading} from './Loading';
import UpdateUser from './UpdateUser';
import AddPainting from './AddPainting';
import {Link, Redirect} from "react-router-dom";
import PaintingService from '../services/PaintingService';
import AlbumsPopup from './AlbumsPopup';
import GenreService from '../services/GenreService';

class UserPage extends Component{
    constructor(props) {
        super(props);

        this.state = {
            content: '',
            followingsOpened: true,
            followersOpened: false,
            isDataLoaded: false,
            isUpdatePopupOpened: false,
            isAddPaintPopupOpened: false,
            showAlbumPopup: false,
            user: null,
            login: '',
            firstname: '',
            secondname: '',
            gender: '',
            birthday: '',
            phone: '',
            country: '',
            email: '',
            followers: [],
            following: [],

            newpaintTitle: '',
            newpaintMaterials: '',
            newpaintPainter: '',
            newpaintPrice: 0,
            newpaintDescr: '',
            newpaintGenreId: 0,
            newpaintImage: null,
        };
    }

    async componentDidMount() {
        const user = await UserService.getUser(this.props.userId);

        this.setState({
            user: user,
            login: user.login,
            firstname: user.firstName,
            secondname: user.secondName,
            gender: user.gender,
            birthday: (new Date(user.birthday)).toLocaleString().substr(0, 10),
            phone: user.phone,
            address: user.address,
            country: user.country,
            email: user.email,
            newlogin: user.login,
            newfirstname: user.firstName,
            newsecondname: user.secondName,
            newgender: user.gender,
            newbirthday: new Date(user.birthday),
            newphone: user.phone,
            newcountry: user.country,
            newemail: user.email,
            newaddress: user.address,
            followers: user.followers,
            followings: user.followings,
            isDataLoaded: true
        });
    }

    handleLoginChange = (e) => {
        this.setState({newlogin: e.target.value})
    }

    handleFNameChange = (e) => {
        this.setState({newfirstname: e.target.value})
    }

    handleSNameChange = (e) => {
        this.setState({newsurname: e.target.value})
    }

    handleGenderChange = (e) => {
        this.setState({newgender: e.target.value})
    }

    handleBirthdayChange = (e) => {
        this.setState({newbirthday: new Date(e.target.value)})
    }

    handlePhoneChange = (e) => {
        this.setState({newphone: e.target.value})
    }

    handleAddressChange = (e) => {
        this.setState({newaddress: e.target.value})
    }

    handleCountryChange = (e) => {
        this.setState({newcountry: e.target.value})
    }

    handleEmailChange = (e) => {
        this.setState({newemail: e.target.value})
    }

    handleFollowingsChange = () => {
        this.setState({followingsOpened: !this.state.followingsOpened});
    }

    handleFollowersChange = () => {
        this.setState({followersOpened: !this.state.followersOpened});
    }

    openUpdateInfoPopup = () => {
        window.scrollTo(0, 0);
        this.setState({isUpdatePopupOpened: true});
    }

    keepUpdateInfoPopup = (e) => {
        e.stopPropagation();
    }

    closeUpdateInfoPopup = () => {
        this.setState({isUpdatePopupOpened: false});
    }

    updateUserInfo = async (e) => {
        window.scrollTo(0, 0);
        e.preventDefault();

        await UserService.updateUserInfo(this.props.userId,
            this.state.newlogin,
            this.state.newfirstname,
            this.state.newsecondname,
            this.state.newgender,
            this.state.newbirthday,
            this.state.newphone,
            this.state.newaddress,
            this.state.newcountry,
            this.state.newemail);

        this.props.authorizeUser(this.state.newlogin, this.props.userId, 1, true);

        this.setState({
            isUpdatePopupOpened: false,
            login: this.state.newlogin,
            firstname: this.state.newfirstname,
            secondname: this.state.newsecondname,
            gender: this.state.newgender,
            birthday: (new Date(this.state.newbirthday)).toLocaleString().substr(0, 10),
            phone: this.state.newphone,
            country: this.state.newcountry,
            email: this.state.newemail,
        })
    }

    openAddPaintPopup = async () => {
        window.scrollTo(0, 0);
        this.setState({isAddPaintPopupOpened: true});
    }

    keepAddPaintPopup = (e) => {
        e.stopPropagation();
    }

    closeAddPaintPopup = () => {
        this.setState({isAddPaintPopupOpened: false});
    }

    handleNewPaintTitleChange = (e) => {
        this.setState({newpaintTitle: e.target.value});
    }

    handleNewPaintMaterialsChange = (e) => {
        this.setState({newpaintMaterials: e.target.value});
    }

    handleNewPaintPainterChange = (e) => {
        this.setState({newpaintPainter: e.target.value});
    }

    handleNewPaintPriceChange = (e) => {
        this.setState({newpaintPrice: +e.target.value});
    }

    handleNewPaintDescrChange = (e) => {
        this.setState({newpaintDescr: e.target.value});
    }

    handleNewPaintGenreIdChange = (e) => {
        this.setState({newpaintGenreId: e.target.value});
    }

    handleNewPaintImageChange = (e) => {
        this.setState({newpaintImage: e.target.files[0]});
    }

    addPainting = async () => {
        window.scrollTo(0, 0);

        await PaintingService.addPainting(
            this.state.newpaintTitle,
            this.state.newpaintMaterials,
            this.state.newpaintPainter,
            this.state.newpaintPrice,
            this.state.newpaintDescr,
            this.state.newpaintGenreId,
            this.state.newpaintImage,
            this.props.userId
        );

        this.setState({
            newpaintTitle: '',
            newpaintMaterials: '',
            newpaintPainter: '',
            newpaintPrice: 0,
            newpaintDescr: '',
            newpaintGenreId: 0,
            newpaintImage: null,
            isAddPaintPopupOpened: false,
        });
    }

    openAlbumsPopup = () => {
        window.scrollTo(0, 0);
        this.setState({showAlbumPopup: true});
    }

    closeAlbumPopup = () => {
        this.setState({showAlbumPopup: false});
    }

    keepAlbumPopup = (e) => {
        if(!this.state.isOpenedNewAlbumPopup){
            e.stopPropagation();
        }
    }

    handleFollowChange = async (userId) => {
        const newFollowingArr = [];
        const newFollowersArr = [];
        let isFollowing = false;

        this.state.followings.forEach(user => {
           if(user.userId === userId){
               const newUser = user;
               isFollowing = user.isFollowing;

               newUser.isFollowing = !user.isFollowing;

               newFollowingArr.push(newUser);
           } else{
               newFollowingArr.push(user);
           }
        });

        this.state.followers.forEach(user => {
            if(user.userId === userId){
                const newUser = user;

                newUser.isFollowing = !user.isFollowing;

                newFollowersArr.push(newUser);
            } else{
                newFollowersArr.push(user);
            }
        });

        if(isFollowing){
            await UserService.unfollowUser(this.props.userId, userId);
        }else{
            await UserService.followUser(this.props.userId, userId);
        }

        this.setState({
            followings: newFollowingArr,
            followers: newFollowersArr
        });
    }

    render() {
        if(this.props.userId !== 0){
            if(this.state.isDataLoaded){
                const updateInfoPopup = this.state.isUpdatePopupOpened ? <UpdateUser
                    closePopup={this.closeUpdateInfoPopup}
                    keepPopup={this.keepUpdateInfoPopup}
                    login={this.state.newlogin}
                    handleLoginChange={this.handleLoginChange}
                    firstname={this.state.newfirstname}
                    handleFNameChange={this.handleFNameChange}
                    secondname={this.state.newsecondname}
                    handleSNameChange={this.handleSNameChange}
                    handleGenderChange={this.handleGenderChange}
                    birthday={this.state.newbirthday}
                    handleBirthdayChange={this.handleBirthdayChange}
                    phone={this.state.newphone}
                    handlePhoneChange={this.handlePhoneChange}
                    email={this.state.newemail}
                    handleEmailChange={this.handleEmailChange}
                    address={this.state.newaddress}
                    handleAddressChange={this.handleAddressChange}
                    handleCountryChange={this.handleCountryChange}
                    updateInfo={this.updateUserInfo}/> : '';

                const addPaintPopup = this.state.isAddPaintPopupOpened ? <AddPainting
                    closePopup={this.closeAddPaintPopup}
                    keepPopup={this.keepAddPaintPopup}
                    title={this.state.newpaintTitle}
                    handleTitle={this.handleNewPaintTitleChange}
                    materials={this.state.newpaintMaterial}
                    handleMaterials={this.handleNewPaintMaterialsChange}
                    description={this.state.newpaintDescr}
                    handleDescription={this.handleNewPaintDescrChange}
                    painter={this.state.newpaintPainter}
                    handlePainter={this.handleNewPaintPainterChange}
                    price={this.state.newpaintPrice}
                    handlePrice={this.handleNewPaintPriceChange}
                    handleGenre={this.handleNewPaintGenreIdChange}
                    handleImage={this.handleNewPaintImageChange}
                    addPainting={this.addPainting}/> : '';

                const albumPopup = this.state.showAlbumPopup
                    ? <AlbumsPopup
                        closePopup={this.closeAlbumPopup}
                        keepPopup={this.keepAlbumPopup}
                        openCreate={this.openCreateAlbumPopup}
                        userId={this.props.userId} /> : '';

                const followings = this.state.followings && this.state.followings.length > 0
                    ? this.state.followings.map(user => {
                        return (
                            <div key={user.userId} className={'user-page-follower'}>
                                <Link to={`/user-paintings/${user.userId}`} onClick={() => this.props.changePageTitle('Paintings')} className={'user-page-follower-name'}>{user.login}</Link>
                                <div className={`user-page-follower-btn ${user.isFollowing ? '' : 'user-page-follow'}`} onClick={() => this.handleFollowChange(user.userId)}>{user.isFollowing ? 'Unfollow' : 'Follow'}</div>
                            </div>
                        );
                    })
                    : <div className={'user-page-follow-notfound'}>Users not found</div>;

                const followers = this.state.followers && this.state.followers.length > 0
                    ? this.state.followers.map(user => {
                        return (
                            <div key={user.userId} className={'user-page-follower'}>
                                <Link to={`/user-paintings/${user.userId}`} onClick={() => this.props.changePageTitle('Paintings')} className={'user-page-follower-name'}>{user.login}</Link>
                                <div className={`user-page-follower-btn ${user.isFollowing ? '' : 'user-page-follow'}`} onClick={() => this.handleFollowChange(user.userId)}>{user.isFollowing ? 'Unfollow' : 'Follow'}</div>
                            </div>
                        );
                    })
                    : <div className={'user-page-follow-notfound'}>Users not found</div>;

                return(
                    <div className={'user-page-container'}>
                        {albumPopup}
                        {updateInfoPopup}
                        {addPaintPopup}
                        <div className={'user-page-left'}>
                            <div className={'user-page-follows'}>
                                <div className={`user-page-follows-title`} onClick={this.handleFollowingsChange}>
                                    Following
                                    <i className={`fa ${this.state.followingsOpened ? 'fa-chevron-up' : 'fa-chevron-down'}`} />
                                </div>
                                <div className={`user-page-follows-users ${this.state.followingsOpened ? 'active' : ''}`}>
                                    {followings}
                                </div>
                            </div>
                            <div className={'user-page-follows'}>
                                <div className={`user-page-follows-title`} onClick={this.handleFollowersChange}>
                                    Followers
                                    <i className={`fa ${this.state.followersOpened ? 'fa-chevron-up' : 'fa-chevron-down'}`} />
                                </div>
                                <div className={`user-page-follows-users ${this.state.followersOpened ? 'active' : ''}`}>
                                    {followers}
                                </div>
                            </div>
                        </div>
                        <div className={'user-page-mid'}>
                            <div className={'user-page-mid-container'}>
                                <div className={'user-page-image-container'}>
                                    <img src={process.env.PUBLIC_URL + '/user.png'} />
                                </div>
                                <div className={'user-page-info'}>
                                    <div className={'user-page-info-item'}>Login: <span>{this.state.login}</span></div>
                                    <div className={'user-page-info-item'}>Name: <span>{this.state.firstname} {this.state.secondname}</span></div>
                                    <div className={'user-page-info-item'}>Gender: <span>{this.state.gender}</span></div>
                                    <div className={'user-page-info-item'}>Birthday: <span>{this.state.birthday}</span></div>
                                    <div className={'user-page-info-item'}>Phone: <span>{this.state.phone}</span></div>
                                    <div className={'user-page-info-item'}>Address: <span>{this.state.address}</span></div>
                                    <div className={'user-page-info-item'}>Country: <span>{this.state.country}</span></div>
                                    <div className={'user-page-info-item'}>Email: <span>{this.state.email}</span></div>
                                </div>
                            </div>
                            <div className={'user-page-info-btn'} onClick={this.openUpdateInfoPopup}>Manage user info</div>
                        </div>
                        <div className={'user-page-right'}>
                            <div className={'user-page-paintings-btns'}>
                                <div className={'user-page-add-painting'} onClick={this.openAddPaintPopup}>Add painting</div>
                                <Link to={'/user/manage-paintings'} className={'user-page-manage-paintings'}>Manage paintings</Link>
                            </div>
                            <Link to={'/user/purchases'} className={'user-page-purchases'}>Purchases</Link>
                            <div className={'user-page-albums'} onClick={this.openAlbumsPopup}>Albums</div>
                            <Link to={'/user/analytics'} className={'user-page-analytics'}>Analytics</Link>
                        </div>
                    </div>
                );
            }else{
                return <Loading/>;
            }
        }else{
            return <Redirect to={'login'} />;
        }
    }
}

export default UserPage;