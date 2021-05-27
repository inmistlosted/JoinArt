import React, {Component} from 'react'
import Cookies from 'universal-cookie/lib';
import "../componentsStyles/LoginStyle.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link, Redirect
} from "react-router-dom";
import UserService from '../services/UserService';

class Login extends Component{
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            password: '',

            isLoginValid: true,
            isPasswordValid: true,
            isFormValid: true,
            useCookies: false,

            invalidMessage: ''
        };
    }

    handleLoginChange = (e) => {
        this.setState({login: e.target.value})
    }

    handlePasswordChange = (e) => {
        this.setState({password: e.target.value})
    }

    handleCookiesChange = (e) => {
        this.setState({useCookies: e.target.checked})
    }

    login = async (e) => {
        e.preventDefault();
        let formValid = true;

        console.log('coo ' + this.state.useCookies);

        if(this.state.login.length === 0){
            this.setState({isLoginValid: false, isFormValid: false, invalidMessage: 'Fill all fields'});
            formValid = false;
        }else{
            this.setState({isLoginValid: true, isFormValid: true, invalidMessage: ''});
        }

        if(this.state.password.length === 0){
            this.setState({isPasswordValid: false, isFormValid: false, invalidMessage: 'Fill all fields'});
            formValid = false;
        }else{
            this.setState({isPasswordValid: true, isFormValid: this.state.isLoginValid, invalidMessage: ''});
        }

        if(formValid){
            try{
                const response = await UserService.loginUser(
                    this.state.login,
                    this.state.password
                );

                if(response.status){
                    this.props.authorizeUser(response.login, response.userId, response.roleId, this.state.useCookies);

                    //window.location.href = '/';
                }else{
                    this.setState({isFormValid: false, invalidMessage: response.message});
                }
            }catch (e) {
                this.setState({isFormValid: false, invalidMessage: 'Unexpected error occurred'});
            }
        }
    }

    render() {
        if(this.props.userLogin.length > 0){
            return (
                <Redirect to={'/'} />
            );
        }else{
            return (
                <div className="container loginpos">
                    <div className="d-flex justify-content-center h-100">
                        <div className={`card card-width-500 ${this.state.isFormValid ? '' : 'login-invalid'}`}>
                            <div className="card-header">
                                <h3>Sign In</h3>
                                <div className="d-flex justify-content-end social_icon">
                                    <span><i className="fa fa-facebook-square"></i></span>
                                    <span><i className="fa fa-google-plus-square"></i></span>
                                    <span><i className="fa fa-twitter-square"></i></span>
                                </div>
                            </div>
                            <div className="card-body">
                                {this.state.isFormValid ? '' : <div className={'login-invalid-message'}>{this.state.invalidMessage}</div>}
                                <form>
                                    <div className={`input-group form-group ${this.state.isLoginValid ? '' : 'login-invalid'}`}>
                                        <div className="input-group-prepend">
                                            <span className="input-group-text"><i className="fa fa-user"></i></span>
                                        </div>
                                        <input type="text" className={`form-control`} placeholder="username" value={this.state.login} onChange={this.handleLoginChange}/>

                                    </div>
                                    <div className={`input-group form-group ${this.state.isPasswordValid ? '' : 'login-invalid'}`}>
                                        <div className="input-group-prepend">
                                            <span className="input-group-text"><i className="fa fa-key"></i></span>
                                        </div>
                                        <input type="password" className={`form-control`} placeholder="password" value={this.state.password} onChange={this.handlePasswordChange}/>
                                    </div>
                                    <div className="row align-items-center remember">
                                        <input type="checkbox" checked={this.state.useCookies} onChange={this.handleCookiesChange}/>Remember Me
                                    </div>
                                    <div className="form-group">
                                        <input type="submit" value="Login" className="btn float-right login_btn" onClick={this.login}/>
                                    </div>
                                </form>
                            </div>
                            <div className="card-footer">
                                <div className="d-flex justify-content-center links">
                                    Don't have an account?<Link to={'/signup'} className={'login-signup-btn'}>Sign Up</Link>
                                </div>
                                <div className="d-flex justify-content-center forgottitle">
                                    <Link to={'/'}>Forgot your password?</Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}

export default Login