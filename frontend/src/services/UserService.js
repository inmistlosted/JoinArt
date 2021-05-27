import axios from 'axios';
import settings from '../settings.json';

class UserService {
    static registerUser(login, firstname, surname, gender, birthday, phone, address, country, email, password){
        const registerModel = new FormData();
        registerModel.append('model.login', login);
        registerModel.append('model.firstname', firstname);
        registerModel.append('model.secondname', surname);
        registerModel.append('model.gender', gender);
        registerModel.append('model.birthday', birthday.toLocaleString());
        registerModel.append('model.phone', phone);
        registerModel.append('model.address', address);
        registerModel.append('model.country', country);
        registerModel.append('model.email', email);
        registerModel.append('model.password', password);

        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/users/register`;

                const res = await axios.post(url, registerModel, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }

    static loginUser(login, password){
        const loginModel = new FormData();
        loginModel.append('model.login', login);
        loginModel.append('model.password', password);

        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/users/login`;

                const res = await axios.post(url, loginModel, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }

    static followUser(followerId, followingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/users/follow/${followerId}/${followingId}`;

                const res = await axios.post(url, null, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }

    static unfollowUser(followerId, followingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/users/unfollow/${followerId}/${followingId}`;

                const res = await axios.post(url, null, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }

    static getUser(userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/users/get-user/${userId}`;

                const res = await axios.get(url, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }

    static updateUserInfo(userId, login, firstname, surname, gender, birthday, phone, address, country, email){
        const model = new FormData();
        model.append('model.userId', userId);
        model.append('model.login', login);
        model.append('model.firstname', firstname);
        model.append('model.secondname', surname);
        model.append('model.gender', gender);
        model.append('model.birthday', birthday.toLocaleString());
        model.append('model.phone', phone);
        model.append('model.address', address);
        model.append('model.country', country);
        model.append('model.email', email);

        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/users/update`;

                const res = await axios.post(url, model, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }

    static getUserInfo(userId, currentUserId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/users/get-user-info/${userId}/${currentUserId}`;

                const res = await axios.get(url, {
                    headers : {
                        'Content-Type' : 'application/x-www-form-urlencoded;'
                    }
                });
                const data = res.data;

                resolve(data);
            } catch (e) {
                reject(e);
            }
        })
    }
}

export default UserService;