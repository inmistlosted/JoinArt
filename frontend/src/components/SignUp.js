import React, {Component} from 'react'
import "../componentsStyles/LoginStyle.css"
import "../componentsStyles/SignUpStyle.css"
import CountryList from './CountryList';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    Redirect
} from "react-router-dom";
import UserService from '../services/UserService';

class SignUp extends Component{
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            firstname: '',
            surname: '',
            gender: 'Male',
            birthday: new Date('1900-01-01'),
            phone: '',
            address: '',
            country: '',
            email: '',
            password: '',
            passwordSubmit: '',

            isValidLogin: true,
            isValidFName: true,
            isValidSName: true,
            isValidPhone: true,
            isValidEmail: true,
            isValidPassword: true,
            isValidPasswordSubmit: true,
            isFormValid: false,
            registerFailure: false,
            registerFailureMessage: ''
        };
    }

    handleLoginChange = (e) => {
        this.setState({login: e.target.value})
    }

    handleFNameChange = (e) => {
        this.setState({firstname: e.target.value})
    }

    handleSNameChange = (e) => {
        this.setState({surname: e.target.value})
    }

    handleGenderChange = (e) => {
        this.setState({gender: e.target.value})
    }

    handleBirthdayChange = (e) => {
        this.setState({birthday: new Date(e.target.value)})
    }

    handlePhoneChange = (e) => {
        this.setState({phone: e.target.value})
    }

    handleAddressChange = (e) => {
        this.setState({address: e.target.value})
    }

    handleCountryChange = (e) => {
        this.setState({country: e.target.value})
    }

    handleEmailChange = (e) => {
        this.setState({email: e.target.value})
    }

    handlePasswordChange = (e) => {
        this.setState({password: e.target.value})
    }

    handlePasswordSubmitChange = (e) => {
        this.setState({passwordSubmit: e.target.value})
    }

    checkLoginValid = () => {
        const login = this.state.login;

        if(login.length < 3){
            this.setState({isValidLogin: false});
        }else{
            this.setState({isValidLogin: true});
        }

        this.isFromValid();
    }

    checkFNameValid = () => {
        const name = this.state.firstname;

        if(name.length === 0){
            this.setState({isValidFName: false});
        }else{
            this.setState({isValidFName: true});
        }

        this.isFromValid();
    }

    checkSNameValid = () => {
        const name = this.state.surname;

        if(name.length === 0){
            this.setState({isValidSName: false});
        }else{
            this.setState({isValidSName: true});
        }

        this.isFromValid();
    }

    checkPhoneValid = () => {
        const phone = this.state.phone;
        const pattern = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;

        if(!pattern.test(phone)){
            this.setState({isValidPhone: false});
        }else{
            this.setState({isValidPhone: true});
        }

        this.isFromValid();
    }

    checkEmailValid = () => {
        const email = this.state.email;
        const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if(!pattern.test(email.toLowerCase())){
            this.setState({isValidEmail: false});
        }else{
            this.setState({isValidEmail: true});
        }

        this.isFromValid();
    }

    checkPasswordValid = () => {
        const password = this.state.password;

        if(password.length < 8){
            this.setState({isValidPassword: false});
        }else{
            this.setState({isValidPassword: true});
        }

        this.isFromValid();
    }

    submitPassword = () => {
        const password = this.state.password;
        const submitPassword = this.state.passwordSubmit;

        if(password !== submitPassword){
            this.setState({isValidPassword: false, isValidPasswordSubmit: false});
        }else{
            this.setState({isValidPassword: true, isValidPasswordSubmit: true});
        }

        this.isFromValid();
    }

    isFromValid = () => {
        const isFormValid = this.state.isValidLogin
            && this.state.isValidFName
            && this.state.isValidSName
            && this.state.isValidPhone
            && this.state.isValidEmail
            && this.state.isValidPassword;

        if(isFormValid){
            this.setState({isFormValid: true});
        }else{
            this.setState({isFormValid: false});
        }
    }

    submitRegister = async (e) => {
        e.preventDefault();

        this.checkPasswordValid();
        this.checkLoginValid();
        this.checkFNameValid();
        this.checkSNameValid();
        this.checkPhoneValid();
        this.checkEmailValid();


        if(this.state.isValidPassword){
            this.submitPassword();
        }


        console.log(this.state.isValidLogin);

        if(this.state.isFormValid){
            try{
                const response = await UserService.registerUser(
                    this.state.login,
                    this.state.firstname,
                    this.state.surname,
                    this.state.gender,
                    this.state.birthday,
                    this.state.phone,
                    this.state.address,
                    this.state.country,
                    this.state.email,
                    this.state.password
                );

                if(response.status){
                    this.props.authorizeUser(response.login, response.userId, response.roleId, false);
                    //window.location.href = '/';
                }else{
                    this.setState({registerFailure: true, registerFailureMessage: response.message});
                }
            }catch (e) {
                this.setState({registerFailure: true, registerFailureMessage: 'Unexpected error occurred'});
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
                        <div className={`card card-width-500 ${this.state.registerFailure ? 'signup-input-error' : ''}`}>
                            <div className="card-header">
                                <h3>Sign Up</h3>
                                <div className="d-flex justify-content-end social_icon">
                                    <span><i className="fa fa-facebook-square"></i></span>
                                    <span><i className="fa fa-google-plus-square"></i></span>
                                    <span><i className="fa fa-twitter-square"></i></span>
                                </div>
                            </div>
                            <div className="card-body">
                                {this.state.registerFailure ? <div className={'signup-failure-message'}>{this.state.registerFailureMessage}</div> : ''}
                                <form>
                                    <div className="input-group form-group signup-input-container">
                                        <input type="text" className={`form-control signup-input ${this.state.isValidLogin ? '' : 'signup-input-error'}`} placeholder="Username" value={this.state.login} onChange={this.handleLoginChange} onBlur={this.checkLoginValid}/>
                                        {this.state.isValidLogin ? '' : <div className={'signup-error-message'}>Login should contain more than 2 characters</div>}
                                    </div>

                                    <div className="input-group form-group signup-input-container">
                                        <input type="text" className={`form-control signup-input ${this.state.isValidFName ? '' : 'signup-input-error'}`} placeholder="Name" value={this.state.firstname} onChange={this.handleFNameChange} onBlur={this.checkFNameValid}/>
                                        {this.state.isValidFName ? '' : <div className={'signup-error-message'}>Name cannot be empty</div>}
                                    </div>

                                    <div className="input-group form-group signup-input-container">
                                        <input type="text" className={`form-control signup-input ${this.state.isValidSName ? '' : 'signup-input-error'}`} placeholder="Surname" value={this.state.surname} onChange={this.handleSNameChange} onBlur={this.checkSNameValid}/>
                                        {this.state.isValidSName ? '' : <div className={'signup-error-message'}>Surname cannot be empty</div>}
                                    </div>

                                    <fieldset id="setD">
                                        <input id="setD_male" type="radio" name="setD_gender" checked={true} value={'Male'} onChange={this.handleGenderChange}/>
                                        <label htmlFor="setD_male" className="gendertitle"> Male </label>
                                        <input id="setD_female" type="radio" name="setD_gender" value={'Female'} onChange={this.handleGenderChange}/>
                                        <label htmlFor="setD_female" className="gendertitle"> Female </label>
                                    </fieldset>

                                    <div className="input-group form-group">
                                        <label htmlFor="birthday" className="gendertitle">Birthday:</label>
                                        <input type="date" id="birthday" name="birthday" onChange={this.handleBirthdayChange}/>
                                    </div>

                                    <div className="input-group form-group signup-input-container">
                                        <input type="text" className={`form-control signup-input ${this.state.isValidPhone ? '' : 'signup-input-error'}`} placeholder="Phone number" value={this.state.phone} onChange={this.handlePhoneChange} onBlur={this.checkPhoneValid}/>
                                        {this.state.isValidPhone ? '' : <div className={'signup-error-message'}>Phone is of invalid format</div>}
                                    </div>

                                    <div className="input-group form-group">
                                        <input type="text" className="form-control" placeholder="Address" value={this.state.address} onChange={this.handleAddressChange}/>
                                    </div>

                                    <div className="form-group">
                                        <CountryList handleCountryChange={this.handleCountryChange}/>
                                    </div>

                                    <div className="input-group form-group signup-input-container">
                                        <input type="text" className={`form-control signup-input ${this.state.isValidEmail ? '' : 'signup-input-error'}`} placeholder="Email" value={this.state.email} onChange={this.handleEmailChange} onBlur={this.checkEmailValid}/>
                                        {this.state.isValidEmail ? '' : <div className={'signup-error-message'}>Email is of invalid format</div>}
                                    </div>

                                    <div className="input-group form-group signup-input-container">
                                        <input type="password" className={`form-control signup-input ${this.state.isValidPassword ? '' : 'signup-input-error'}`} placeholder="Password" value={this.state.password} onChange={this.handlePasswordChange} onBlur={this.checkPasswordValid}/>
                                        {this.state.isValidPassword ? '' : <div className={'signup-error-message'}>Password should contain more than 7 characters</div>}
                                    </div>

                                    <div className="input-group form-group signup-input-container">
                                        <input type="password" className={`form-control signup-input ${this.state.isValidPasswordSubmit ? '' : 'signup-input-error'}`} placeholder="Confirm password" onChange={this.handlePasswordSubmitChange} onBlur={this.submitPassword}/>
                                        {this.state.isValidPasswordSubmit ? '' : <div className={'signup-error-message'}>Password and password confirmation should be equal</div>}
                                    </div>

                                    <div className="form-group">
                                        <input type="submit" value="Sign Up" className="btn float-right login_btn" onClick={this.submitRegister}/>
                                    </div>
                                </form>
                            </div>
                            <div className="card-footer">


                            </div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}

export default SignUp