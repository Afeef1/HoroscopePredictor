const jwt = require('jsonwebtoken');
const payload ={"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Afeef",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "1",
            
            "iss": "http://horoscopeapi",
            "aud": "http://horoscopeapi"}
function generateToken() {
    const secret = 'AGRM0D21SldLSnsj93JDjmddsagdwuLDUDNDdja'; 
    return jwt.sign(payload, secret,{expiresIn:'1h'}); 
}

 module.exports = { generateToken };