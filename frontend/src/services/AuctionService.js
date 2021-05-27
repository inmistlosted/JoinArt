import axios from 'axios';
import settings from '../settings.json';

class AuctionService {
    static getPaintingBidId(paintingId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/auctions/get-painting-bid/${paintingId}`;

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

    static getBid(bidId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/auctions/get-bid/${bidId}`;

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

    static startBid(bidId, userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/auctions/start-bid/${bidId}/${userId}`;

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

    static placeBet(bidId, userId, price){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/auctions/place-bet/${bidId}/${userId}/${price}`;

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

    static getTopAuctions(){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/auctions/get-top-bids`;

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

    static getAllAuctions(){
        return new Promise(async (resolve, reject) => {
            try {
                const url =  `${settings.apiUrl}/auctions/get-all-bids`;

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

export default AuctionService;