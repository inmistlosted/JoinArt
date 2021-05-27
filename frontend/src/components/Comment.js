import React, {Component} from 'react'
import "../componentsStyles/Comment.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

class Comment extends Component{
    render() {
        return (
           <div key={this.props.id} className={"container commentsize"}>
               <div className={'comment-line-v'} />
               <div className={'comment-line-h'} />
               <div className={"row comment-body"}>
                   <div className={"col-md-2 imgsize"}>
                       <img height={"75px"} width={"80px"} src={process.env.PUBLIC_URL + '/user.png'} />
                   </div>
                   <div className={"col-md-10 commtext"}>
                       <h4>{this.props.username}</h4>
                       <div className="comment-content">
                           {this.props.content}
                           <div className={'comment-date'}>{(new Date(this.props.date)).toLocaleString()}</div>
                       </div>
                   </div>
               </div>
           </div>

        );
    }
}

export default Comment