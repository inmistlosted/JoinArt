import axios from 'axios';
import settings from '../settings.json';

class OrderService {
    static getUserOrders(userId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/orders/get-user-orders/${userId}`;

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

    static submitOrder(orderId){
        return new Promise(async (resolve, reject) => {
            try {
                const url = `${settings.apiUrl}/orders/pay-order/${orderId}`;

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

export default OrderService;