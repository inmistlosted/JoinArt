import axios from 'axios';
import settings from '../settings.json';

class PaintingService {
    static getPainting(paintingId, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/paintings/get-painting/${paintingId}/${userId}`;

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

    static addLike(paintingId, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/paintings/add-like/${paintingId}/${userId}`;

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

    static removeLike(paintingId, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/paintings/remove-like/${paintingId}/${userId}`;

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

    static addComment(paintingId, userId, content){
        return new Promise(async (resolve, reject) => {
            try {
                const commentModel = new FormData();
                commentModel.append('model.paintingId', paintingId);
                commentModel.append('model.userId', userId);
                commentModel.append('model.content', content);

                const url = `${settings.apiUrl}/paintings/add-comment`;

                const res = await axios.post(url, commentModel, {
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

    static searchPaintings(query, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/paintings/search/${query}/${userId}`;

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

    static getTopPaintings(userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/paintings/get-top-paintings/${userId}`;

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

    static getAllPaintings(userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/paintings/get-all/${userId}`;

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

    static addPainting(title, materials, painter, price, description, genreId, image, ownerId){
        const model = new FormData();
        model.append('model.title', title);
        model.append('model.materials', materials);
        model.append('model.painter', painter);
        model.append('model.price', price);
        model.append('model.description', description);
        model.append('model.genresIds', genreId);
        model.append('model.image', image);
        model.append('model.ownerId', ownerId);

        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/paintings/add-painting`;

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

    static getUserPaintings(userId, currentUserId){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/paintings/get-user-paintings/${userId}/${currentUserId}`;

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
        });
    }

    static deletePainting(paintingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/paintings/delete-painting/${paintingId}`;

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

    static updatePainting(paintingId, title, materials, painter, price, description, genreId, image){
        const model = new FormData();
        model.append('model.paintingId', paintingId);
        model.append('model.title', title);
        model.append('model.materials', materials);
        model.append('model.painter', painter);
        model.append('model.price', price);
        model.append('model.description', description);
        model.append('model.genresIds', genreId);
        model.append('model.image', image);

        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/paintings/update-painting`;

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
}

export default PaintingService;