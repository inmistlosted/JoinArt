import axios from 'axios';
import settings from '../settings.json';

class AlbumService {
    static getUserAlbums(userId, paintingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/get-user-albums/${userId}/${paintingId}`;

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

    static getUserAllAlbums(userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/get-user-albums/${userId}`;

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

    static create(title, description, userId){
        const model = new FormData();
        model.append('model.title', title);
        model.append('model.description', description);
        model.append('model.userId', userId);

        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/albums/create`;

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

    static getTopAlbums(){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/albums/get-top-albums`;

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

    static getAllAlbums(){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/albums/get-all-albums`;

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

    static getAlbum(albumId, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/get-album/${albumId}/${userId}`;

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

    static addToAlbum(albumId, paintingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/add-painting-to-album/${albumId}/${paintingId}`;

                const res = await axios.post(url, {}, {
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

    static removeFromAlbum(albumId, paintingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/remove-painting-from-album/${albumId}/${paintingId}`;

                const res = await axios.post(url, {}, {
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

    static update(albumId, title, description){
        const model = new FormData();
        model.append('model.title', title);
        model.append('model.description', description);
        model.append('model.albumId', albumId);

        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/update`;

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

    static delete(albumId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/albums/delete/${albumId}`;

                const res = await axios.post(url, {}, {
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

export default AlbumService;