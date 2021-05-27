import React, {Component} from 'react';
import '../../componentsStyles/Painting.css';
import PaintingService from '../../services/PaintingService';
import InfoPopup from '../InfoPopup';
import {Link, NavLink} from 'react-router-dom';
import AlbumService from '../../services/AlbumService';

class Painting extends Component{
    constructor(props) {
        super(props);

        this.state = {
            likesCount: this.props.likesCount,
            isLiked: this.props.isLiked,
            showPopup: false,
            popupMessage: ''
        };
    }

    handleLike = async (e) => {
        e.stopPropagation();

        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({popupMessage: 'Please login first', showPopup: true});
        }else{
            if(this.state.isLiked){
                await PaintingService.removeLike(this.props.id, this.props.userId);
                this.setState({isLiked: false, likesCount: this.state.likesCount-1});
            }else{
                await PaintingService.addLike(this.props.id, this.props.userId);
                this.setState({isLiked: true, likesCount: this.state.likesCount+1});
            }
        }
    }

    closePopup = () => {
        this.setState({showPopup: false});
    }

    render() {
        const imageSrc = this.props.image ? this.props.image : process.env.PUBLIC_URL + '/pic.jpg';
        const popup = this.state.showPopup ? <InfoPopup message={this.state.popupMessage} closePopup={this.closePopup}/> : '';
        const removeFromAlbumBtn = this.props.belongsToUser
            ?   <i className={'fa fa-trash painting-remove'} onClick={() => this.props.removeFromAlbum(this.props.id)}>
                    <div className={'painting-remove-tooltip'}>Remove from album</div>
                </i>
            : '';

        return(
            <div className={`painting ${this.props.belongsToUser ? 'painting-relative' : ''} ${this.props.isSmaller ? 'small' : ''}`}>
                {popup}
                <Link to={`../painting/${this.props.id}`} className={`painting-image ${this.props.isSmaller ? 'small' : ''}`} onClick={() => this.props.changePageTitle('Paintings')}>
                    <img src={imageSrc} />
                </Link>
                <div className={'painting-info'}>
                    <Link to={`../painting/${this.props.id}`} className={`painting-title ${this.props.isSmaller ? 'small' : ''}`} onClick={() => this.props.changePageTitle('Paintings')}>{this.props.title}</Link>
                    <div className={`painting-author ${this.props.isSmaller ? 'small' : ''}`}>{this.props.author}</div>
                    <div className={`painting-price ${this.props.isSmaller ? 'small' : ''}`}>{this.props.status ? `$${this.props.price}` : 'Sold'}</div>
                    <div className={`painting-likes ${this.props.isSmaller ? 'small' : ''}`}>
                        <i className={`fa fa-heart painting-like ${this.state.isLiked ? 'active' : ''} ${this.props.isSmaller ? 'small' : ''}`} onClick={this.handleLike}/>
                        Likes: <span className={`painting-likes-count ${this.props.isSmaller ? 'small' : ''}`}>{this.state.likesCount}</span>
                        <span className={`painting-separator ${this.props.isSmaller ? 'small' : ''}`}>|</span>
                        Comments: <span className={`painting-comments-count ${this.props.isSmaller ? 'small' : ''}`}>{this.props.commentsCount}</span>
                    </div>
                </div>
                {removeFromAlbumBtn}
            </div>
        );
    }
}

export default Painting;