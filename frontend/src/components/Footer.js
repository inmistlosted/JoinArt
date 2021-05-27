import React, {Component} from 'react'
import "../componentsStyles/FooterStyle.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

class Footer extends Component{

    render() {

        return (
                <nav className=" footercolor footeralgn footer-bot-mrg">
                    <div className="row text-center text-xs-center text-sm-left text-md-left footer-mrg-0">
                        <div className="col-xs-12 col-sm-4 col-md-4 textal textcol">
                            <h5>Quick links</h5>
                            <ul className="list-unstyled quick-links">
                                <li><Link to={'/'}><i className="fa fa-angle-double-right" />Home</Link></li>
                                <li><Link to={'/genres'}><i className="fa fa-angle-double-right" />Genres</Link></li>
                                <li><Link to={'/albums'}><i className="fa fa-angle-double-right" />Albums</Link></li>
                                <li><Link to={'/auctions'}><i className="fa fa-angle-double-right" />Auctions</Link></li>
                            </ul>
                        </div>
                        <div className="col-xs-12 col-sm-4 col-md-4 textal textcol">
                            <h5>Quick links</h5>
                            <ul className="list-unstyled quick-links">
                                <li><Link to={'/'}><i className="fa fa-angle-double-right" />Home</Link></li>
                                <li><Link to={'/genres'}><i className="fa fa-angle-double-right" />Genres</Link></li>
                                <li><Link to={'/albums'}><i className="fa fa-angle-double-right" />Albums</Link></li>
                                <li><Link to={'/auctions'}><i className="fa fa-angle-double-right" />Auctions</Link></li>
                            </ul>
                        </div>
                        <div className="col-xs-12 col-sm-4 col-md-4 textal textcol">
                            <h5>Quick links</h5>
                            <ul className="list-unstyled quick-links">
                                <li><Link to={'/'}><i className="fa fa-angle-double-right" />Home</Link></li>
                                <li><Link to={'/genres'}><i className="fa fa-angle-double-right" />Genres</Link></li>
                                <li><Link to={'/albums'}><i className="fa fa-angle-double-right" />Albums</Link></li>
                                <li><Link to={'/auctions'}><i className="fa fa-angle-double-right" />Auctions</Link></li>
                            </ul>
                        </div>
                    </div>
                    <div className="row footer-mrg-0">
                        <div className="col-xs-12 col-sm-12 col-md-12 mt-2 mt-sm-5 textcol">
                            <ul className="list-unstyled list-inline social text-center">
                                <li className="list-inline-item"><a href="#"><i
                                    className="fa fa-facebook" /></a></li>
                                <li className="list-inline-item"><a href="#"><i
                                    className="fa fa-twitter" /></a></li>
                                <li className="list-inline-item"><a href="#"><i
                                    className="fa fa-instagram" /></a></li>
                                <li className="list-inline-item"><a href="#"><i
                                    className="fa fa-google-plus" /></a></li>
                                <li className="list-inline-item"><a href="#"
                                                                    target="_blank"><i
                                    className="fa fa-envelope" /></a></li>
                            </ul>
                        </div>
                        <hr/>
                    </div>
                    <div className="row footer-mrg-0">
                        <div className="col-xs-12 col-sm-12 col-md-12 mt-2 mt-sm-2 text-center text-white">
                            <p><u><a href="#">National ArtHouse
                                Corporation</a></u> dedicated to emerging artists</p>
                            <p className="h6">© All right Reversed.<a className="text-green ml-2"
                                                                      href="#"
                                                                      target="_blank">JoinArt</a></p>
                        </div>
                        <hr/>
                    </div>
                </nav>
        );
    }
}


export default Footer