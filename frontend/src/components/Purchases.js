import React, {Component} from 'react'
import "../componentsStyles/ManagePaintings.css"
import {Redirect} from "react-router-dom";
import Receipt from './Receipt';
import {Loading} from './Loading';
import OrderService from '../services/OrderService';

class Purchases extends Component{
    constructor(props) {
        super(props);

        this.state = {
            isReceiptOpen: false,
            orders: [],
            isDataLoaded: false
        };
    }

    async componentDidMount() {
        const orders = await OrderService.getUserOrders(this.props.userId);
        console.log(orders);

        this.setState({
            isDataLoaded: true,
            orders: orders,

            receiptOrderId: 0,
            receiptItemTitle: '',
            receiptItemCount: 0,
            receiptItemPrice: 0,
            receiptAmount: 0
        });
    }

    closeReceipt = () => {
        this.setState({isReceiptOpen: false});
    }

    keepReceipt = (e) => {
        e.stopPropagation();
    }

    openReceipt = (orderId) => {
        window.scrollTo(0, 0);

        this.state.orders.forEach(order => {
           if(order.orderId === orderId){
               this.setState({
                   receiptOrderId: orderId,
                   receiptItemTitle: order.orderItem.painting.title,
                   receiptItemCount: order.orderItem.count,
                   receiptItemPrice: order.orderItem.painting.price,
                   receiptAmount: order.amount,
                   isReceiptOpen: true
               });
           }
        });
    }

    submitOrder = async (orderId) => {
        await OrderService.submitOrder(orderId);

        const newOrdersArr = [];

        this.state.orders.forEach(order => {
            if(order.orderId === orderId){
                const currOrder = order;

                currOrder.status = true;

                newOrdersArr.push(currOrder);
            }else{
                newOrdersArr.push(order);
            }
        });

        this.setState({
            receiptOrderId: 0,
            receiptItemTitle: '',
            receiptItemCount: 0,
            receiptItemPrice: 0,
            receiptAmount: 0,
            orders: newOrdersArr,
            isReceiptOpen: false
        });
    }

    render() {
        if(this.props.userId !== 0){
            if(this.state.isDataLoaded){
                const orders = this.state.orders != null && this.state.orders.length > 0
                    ? this.state.orders.map((order) => {
                        return(
                            <div key={order.orderId} className={'manage-painting-item'}>
                                <div className={'item pur-order'}>Order #{order.orderId}</div>
                                <div className={'item pur-image'}>
                                    <img src={order.orderItem.painting.imagePath}/>
                                </div>
                                <div className={'item pur-title'}>{order.orderItem.painting.title}</div>
                                <div className={'item pur-price'}>${order.amount}</div>
                                <div className={'item pur-btn-container'}>
                                    {
                                        order.status
                                            ? <div className={'pur-paid'}>Paid <i className={'fa fa-check'}/></div>
                                            : <div className={'buy-btn'} onClick={() => this.openReceipt(order.orderId)}>Pay</div>
                                    }
                                </div>
                            </div>
                        );
                    })
                    : <div className={'genre-no-paintings-found'}>No purchases found</div>;

                const receipt = this.state.isReceiptOpen ? <Receipt
                    closeReceipt={this.closeReceipt}
                    keepReceipt={this.keepReceipt}
                    orderId={this.state.receiptOrderId}
                    itemTitle={this.state.receiptItemTitle}
                    itemPrice={this.state.receiptItemPrice}
                    amount={this.state.receiptAmount}
                    itemCount={this.state.receiptItemCount}
                    submitOrder={this.submitOrder}/> : '';

                return (
                    <div className={`manage-paintings ${this.state.isReceiptOpen ? 'manage-paintings-mrg' : ''}`}>
                        {receipt}
                        <div className={'purchases-title'}>Purchases</div>
                        <div className={'manage-paintings-container pur-container'}>
                            <div className={'manage-paintings-paints'}>
                                {orders}
                            </div>
                        </div>
                    </div>
                );
            }else{
                return <Loading />;
            }
        }else{
            return <Redirect to={'/login'} />;
        }
    }
}

export default Purchases;