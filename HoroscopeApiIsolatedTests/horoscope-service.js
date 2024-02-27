const { addStub}=require("./mountebank-helper.js")
function addValidService(zodiac,day) {
    const response = {
        "sun_sign": `${zodiac}`,
        "prediction_date": "6-2-2024",
        "prediction": {
            "personal_life": `${day}, with the Moon in Sagittarius squaring Neptune in Pisces, casts a warm and enchanting light on your relationships. If you're planning a date, expect a delightful time, but remember to keep an open mind about your partner. Avoid making quick judgments and enjoy the magic of the moment.`,
            "profession": `${day},In your professional life, it's a day to bridge gaps. Work closely with those you might not usually get along with, especially those who seem arrogant. Setting aside personal differences and collaborating effectively could lead to significant rewards and progress in your career today.`,
            "health": `${day}'s aspect encourages you to stay active and connected with your physical needs. Engage in activities like gym workouts or yoga, and stay hydrated. This physical engagement helps you remain emotionally balanced, particularly useful when this transit stirs up tensions or miscommunications.`,
            "travel": `Travel ${day} may bring unexpected joys and challenges. Adopt the adventure and use this as an opportunity to learn and grow. Your flexibility and adaptability will be key to making the most of your travels.`,
            "luck": `Luck ${day} lies in your ability to adapt and maintain an open mind. Unexpected opportunities may arise, particularly through interactions with others. Stay receptive to new ideas and perspectives.`,
            "emotions": `You may experience a heightened sense of connection with your emotions ${day}. Grasp this as an opportunity to understand yourself and your relationships better. Staying flexible and open-minded will help you navigate any emotional turbulence that may arise.`
        }
    }

    const stubs = 
        [{
            predicates: [{
                equals: {
                    method: "POST",
                    "path": `/v1/sun_sign_prediction/daily/${zodiac}`,
                }
            }],
            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(response)
                    }
                }
            ]
        },

            {
                predicates: [{
                    equals: {
                        method: "POST",
                        "path": `/v1/sun_sign_prediction/daily/next/${zodiac}`,
                    }
                }],
                responses: [
                    {
                        is: {
                            statusCode: 200,
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(response)
                        }
                    }
                ]
            },

            {
                predicates: [{
                    equals: {
                        method: "POST",
                        "path": `/v1/sun_sign_prediction/daily/previous/${zodiac}`,
                    }
                }],
                responses: [
                    {
                        is: {
                            statusCode: 200,
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(response)
                        }
                    }
                ]
            }
        ];
    

   
     addStub(stubs);
}

function addInvalidService(zodiac) {
    const response = {
        "status": false,
        "error": "ERROR! Please check the zodiac name and spelling."   
    }

    const stubs = 
        [{
            predicates: [{
                equals: {
                    method: "POST",
                    "path": `/v1/sun_sign_prediction/daily/${zodiac}`,
                }
            }],
            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(response)
                    }
                }
            ]
        },

            {
                predicates: [{
                    equals: {
                        method: "POST",
                        "path": `/v1/sun_sign_prediction/daily/next/${zodiac}`,
                    }
                }],
                responses: [
                    {
                        is: {
                            statusCode: 200,
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(response)
                        }
                    }
                ]
            },

            {
                predicates: [{
                    equals: {
                        method: "POST",
                        "path": `/v1/sun_sign_prediction/daily/previous/${zodiac}`,
                    }
                }],
                responses: [
                    {
                        is: {
                            statusCode: 200,
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(response)
                        }
                    }
                ]
            }
        ];
    

   
     addStub(stubs);
}
module.exports = { addValidService,addInvalidService };