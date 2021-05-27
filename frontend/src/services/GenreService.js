import axios from 'axios';
import settings from '../settings.json';

class GenreService {
    static getAllGenresAndMovements(){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/genres/get-all-genres-and-movements`;

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

    static getTopGenres(){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/genres/get-top-genres`;

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

    static getTopMovements(){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/genres/get-top-movements`;

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

    static getGenre(genreId, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/genres/get-genre/${genreId}/${userId}`;

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

    static getTopGenresAndMovements(){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/genres/get-top-genres-and-movements`;

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

export default GenreService;