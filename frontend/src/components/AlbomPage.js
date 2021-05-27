import React, {Component} from 'react'
import "../componentsStyles/AlbomPage.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";
import Albums from "./Albums";

class AlbomPage extends Component{

    constructor(props){
        super(props);
        this.state = {
            options: ["Date","Rating"]
    }
    }


    render() {

        return (


            <div className="container containerstyle border border-warning borderst">





                <div className="row ">
                    <div className="col-md-2 alltopicstitle">
                        <h4>All albums</h4>
                    </div>
                    <div className="col-md-2 alltopicstitle">
                        <h4>Sort by: </h4>
                    </div>
                    <div className={"col-md-8 alltopicstitle"}>
                        <select>{this.state.options.map((option, idx) => <option key={idx}>{option}</option>)}</select>

                    </div>




                    <div className="row">
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>

                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>

                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>

                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>

                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                    <div className="col-md-3">
                        <Albums/>
                    </div>
                </div>
            </div>

        );
    }
}

export default AlbomPage