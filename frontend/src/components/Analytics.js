import React, {Component} from 'react';
import { Bar, Line } from 'react-chartjs-2';
import "../componentsStyles/Analytics.css";

class Analytics extends Component{
    constructor(props) {
        super(props);
    }

    getTopGenresChartData = () => {
        const data = {
            labels: ['Scenery', 'Marine', 'Portrait', 'Still life', 'Historical', 'Battle'],
            datasets: [
                {
                    label: 'Rating',
                    data: [231, 312, 734, 450, 432, 398],
                    backgroundColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)',
                    ],
                    borderWidth: 1,
                },
            ],
        };

        return data;
    }

    getTopGenresChartOptions = () => {
        const options = {
            scales: {
                y: {
                        ticks: {
                            color: 'white',
                        },
                        grid: {
                            color: 'white'
                        }
                },
                x: {
                    ticks: {
                        color: 'white'
                    },
                    grid: {
                        color: 'white'
                    }
                }

            },
            color: 'white',
            plugins: {
                title: {
                    display: true,
                    text: 'Top genres',
                    color: 'white',
                    font: {
                        size: '17px'
                    }
                }
            }
        };

        return options;
    }

    getTopMovementsChartData = () => {
        const data = {
            labels: ['Impressionism', 'Romanticism', 'Renaissance', 'Mannerism', 'Baroque', 'Classicism'],
            datasets: [
                {
                    label: 'Rating',
                    data: [983, 421, 523, 894, 642, 701],
                    backgroundColor: [
                        '#E32636',
                        '#9F2B68',
                        '#44944A',
                        '#FAE7B5',
                        '#FFDC33',
                        '#6495ED',
                    ],
                    borderWidth: 1,
                },
            ],
        };

        return data;
    }

    getTopMovementsChartOptions = () => {
        const options = {
            scales: {
                y: {
                    ticks: {
                        color: 'white',
                    },
                    grid: {
                        color: 'white'
                    }
                },
                x: {
                    ticks: {
                        color: 'white'
                    },
                    grid: {
                        color: 'white'
                    }
                }

            },
            color: 'white',
            plugins: {
                title: {
                    display: true,
                    text: 'Top movements',
                    color: 'white',
                    font: {
                        size: '17px'
                    }
                }
            }
        };

        return options;
    }

    getPaintsChartData = () => {
        const data = {
            labels: ['A deep dream', 'Moroccan motives', 'The untold stories', 'The battle of love'],
            datasets: [
                {
                    label: 'Rating',
                    data: [89, 124, 231, 56],
                    backgroundColor: [
                        '#DE4C8A',
                        '#00FF7F',
                        '#0095B6',
                        '#FF4040'
                    ],
                    borderWidth: 1,
                },
            ],
        };

        return data;
    }

    getPaintsChartOptions = () => {
        const options = {
            indexAxis: 'y',
            scales: {
                y: {
                    ticks: {
                        color: 'white',
                    },
                    grid: {
                        color: 'white'
                    }
                },
                x: {
                    ticks: {
                        color: 'white'
                    },
                    grid: {
                        color: 'white'
                    }
                }

            },
            color: 'white',
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'My paintings rating',
                    color: 'white',
                    font: {
                        size: '17px'
                    }
                },
                legend: {
                    position: 'right',
                },
            }
        };

        return options;
    }

    getPaintsTimeChartData = () => {
        const data = {
            labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            datasets: [
                {
                    label: 'A deep dream',
                    data: [12, 23, 15, 9, 4, 5, 4],
                    backgroundColor: '#F64A46',
                    borderColor: '#F64A46',
                    borderWidth: 3,
                },
                {
                    label: 'Moroccan motives',
                    data: [14, 37, 24, 13, 11, 10, 14],
                    backgroundColor: '#423189',
                    borderColor: '#423189',
                    borderWidth: 3,
                },
                {
                    label: 'The untold stories',
                    data: [34, 57, 45, 34, 28, 25, 24],
                    backgroundColor: '#EFD334',
                    borderColor: '#EFD334',
                    borderWidth: 3,
                },
                {
                    label: 'The battle of love',
                    data: [3, 11, 14, 18, 9, 7, 7],
                    backgroundColor: '#009A63',
                    borderColor: '#009A63',
                    borderWidth: 3,
                },
            ],
        };

        return data;
    }

    getPaintsTimeChartOptions = () => {
        const options = {
            scales: {
                y: {
                    ticks: {
                        color: 'white',
                    },
                    grid: {
                        color: 'white'
                    }
                },
                x: {
                    ticks: {
                        color: 'white'
                    },
                    grid: {
                        color: 'white'
                    }
                }

            },
            color: 'white',
            plugins: {
                title: {
                    display: true,
                    text: 'My paintings views',
                    color: 'white',
                    font: {
                        size: '17px'
                    }
                }
            }
        };

        return options;
    }

    render() {
        return (
            <div className={'charts-outer'}>
                <div className={'charts-btn-container'}>
                    <div className={'charts-btn-inner'}>
                        <div className={'charts-btn active'}>All charts</div>
                        <div className={'charts-btn'}>Top genres</div>
                        <div className={'charts-btn'}>Top moves</div>
                        <div className={'charts-btn'}>My paintings rating</div>
                        <div className={'charts-btn'}>My paintings rating change</div>
                    </div>
                </div>
                <div className={'charts-container'}>
                    <div className={'chart genres-chart'}>
                        <Bar data={this.getTopGenresChartData()} options={this.getTopGenresChartOptions()} />
                    </div>
                    <div className={'chart moves-chart'}>
                        <Bar data={this.getTopMovementsChartData()} options={this.getTopMovementsChartOptions()} />
                    </div>
                    <div className={'chart paints-chart'}>
                        <Bar data={this.getPaintsChartData()} options={this.getPaintsChartOptions()} />
                    </div>
                    <div className={'chart paints-time-chart'}>
                        <Line data={this.getPaintsTimeChartData()} options={this.getPaintsTimeChartOptions()} />
                    </div>
                </div>
            </div>
        );
    }
}

export default Analytics;