import React, {Component} from 'react'
import "../componentsStyles/AlbumCard.css"
import {Link} from 'react-router-dom';

class AlbumCard extends Component{

    render() {
        const imageSrc = this.props.image ? this.props.image : process.env.PUBLIC_URL + '/pic.jpg';

        if(this.props.fromUserPage){
            return (
                <div className={`album-card-user`}>
                    <Link to={`/album/${this.props.id}`}>
                        <img className={`album-card-img`} src={imageSrc} alt="Card image cap" />
                    </Link>
                    <div className="album-card-body">
                        <Link to={`/album/${this.props.id}`} className={`album-card-title`}>{this.props.title}</Link>
                    </div>
                    <div className={'album-card-controls'}>
                        <i className={'fa fa-edit'} onClick={() => this.props.openEdit(this.props.id, this.props.title, this.props.description)}/>
                        <i className={'fa fa-trash'} onClick={() => this.props.delete(this.props.id)}/>
                    </div>
                </div>
            );
        }else{
            return (
                <div className={`album-card`} onClick={() => this.props.chooseAlbum(this.props.id, this.props.title)}>
                    <img className={`album-card-img`} src={imageSrc} alt="Card image cap" />
                    <div className="album-card-body">
                        <h5 className={`album-card-title`}>{this.props.title}</h5>
                    </div>
                    {this.props.isAdded ? <i className={'fa fa-check-circle album-card-added'}/> : ''}
                </div>
            );
        }
    }
}

export default AlbumCard;