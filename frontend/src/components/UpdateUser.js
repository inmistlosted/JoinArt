import React, {Component} from 'react';
import '../componentsStyles/UpdateUser.css';
import CountryList from './CountryList';

class UpdateUser extends Component{
    constructor(props) {
        super(props);

        this.state = {
            isValidLogin: true,
            isValidFName: true,
            isValidSName: true,
            isValidPhone: true,
            isValidEmail: true
        };
    }

    checkLoginValid = () => {
        const login = this.props.login;

        if(login.length < 3){
            this.setState({isValidLogin: false});
        }else{
            this.setState({isValidLogin: true});
        }
    }

    checkFNameValid = () => {
        const name = this.props.firstname;

        if(name.length === 0){
            this.setState({isValidFName: false});
        }else{
            this.setState({isValidFName: true});
        }
    }

    checkSNameValid = () => {
        const name = this.props.secondname;

        if(name.length === 0){
            this.setState({isValidSName: false});
        }else{
            this.setState({isValidSName: true});
        }
    }

    checkPhoneValid = () => {
        const phone = this.props.phone;
        const pattern = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;

        if(!pattern.test(phone)){
            this.setState({isValidPhone: false});
        }else{
            this.setState({isValidPhone: true});
        }
    }

    checkEmailValid = () => {
        const email = this.props.email;
        const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if(!pattern.test(email.toLowerCase())){
            this.setState({isValidEmail: false});
        }else{
            this.setState({isValidEmail: true});
        }
    }

    render() {
        return(
            <div className={'update-user-container'} onClick={this.props.closePopup}>
                <div className={'update-user'} onClick={this.props.keepPopup}>
                    <div className={'update-user-title'}>Update user info</div>
                    <form>
                        <div className="update-user-input">
                            Login
                            <input type="text" className={`form-control signup-input ${this.state.isValidLogin ? '' : 'signup-input-error'}`} placeholder="Username" value={this.props.login} onChange={this.props.handleLoginChange} onBlur={this.checkLoginValid}/>
                            {this.state.isValidLogin ? '' : <div className={'signup-error-message'}>Login should contain more than 2 characters</div>}
                        </div>

                        <div className="update-user-input">
                            First name
                            <input type="text" className={`form-control signup-input ${this.state.isValidFName ? '' : 'signup-input-error'}`} placeholder="Name" value={this.props.firstname} onChange={this.props.handleFNameChange} onBlur={this.checkFNameValid}/>
                            {this.state.isValidFName ? '' : <div className={'signup-error-message'}>Name cannot be empty</div>}
                        </div>

                        <div className="update-user-input">
                            Surname
                            <input type="text" className={`form-control signup-input ${this.state.isValidSName ? '' : 'signup-input-error'}`} placeholder="Surname" value={this.props.surname} onChange={this.props.handleSNameChange} onBlur={this.checkSNameValid}/>
                            {this.state.isValidSName ? '' : <div className={'signup-error-message'}>Surname cannot be empty</div>}
                        </div>

                        <fieldset id="setD" className={'update-user-input'}>
                            <input id="setD_male" type="radio" name="setD_gender" checked={true} value={'Male'} onChange={this.props.handleGenderChange}/>
                            <label htmlFor="setD_male" className="gendertitle"> Male </label>
                            <input id="setD_female" type="radio" name="setD_gender" value={'Female'} onChange={this.props.handleGenderChange}/>
                            <label htmlFor="setD_female" className="gendertitle"> Female </label>
                        </fieldset>

                        <div className="input-group form-group aaa update-user-input">
                            <label htmlFor="birthday" className="gendertitle">Birthday</label>
                            <input type="date" id="birthday" name="birthday" onChange={this.props.handleBirthdayChange}/>
                        </div>

                        <div className="update-user-input">
                            Phone
                            <input type="text" className={`form-control signup-input ${this.state.isValidPhone ? '' : 'signup-input-error'}`} placeholder="Phone number" value={this.props.phone} onChange={this.props.handlePhoneChange} onBlur={this.checkPhoneValid}/>
                            {this.state.isValidPhone ? '' : <div className={'signup-error-message'}>Phone is of invalid format</div>}
                        </div>

                        <div className="update-user-input">
                            Address
                            <input type="text" className="form-control" placeholder="Address" value={this.props.address} onChange={this.props.handleAddressChange}/>
                        </div>

                        <div className="form-group">
                            Country
                            <CountryList handleCountryChange={this.props.handleCountryChange}/>
                        </div>

                        <div className="update-user-input">
                            Email
                            <input type="text" className={`form-control signup-input ${this.state.isValidEmail ? '' : 'signup-input-error'}`} placeholder="Email" value={this.props.email} onChange={this.props.handleEmailChange} onBlur={this.checkEmailValid}/>
                            {this.state.isValidEmail ? '' : <div className={'signup-error-message'}>Email is of invalid format</div>}
                        </div>

                        <div className="form-group">
                            <input type="submit" value="Update" className="btn login_btn update-page-btn" onClick={this.props.updateInfo}/>
                        </div>
                    </form>
                </div>
            </div>
        );
    }
}

export default UpdateUser;