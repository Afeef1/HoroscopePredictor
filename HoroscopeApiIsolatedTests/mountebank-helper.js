function postImposter(body) {
    const url = "http://localhost:2525/imposters"

    return fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    });

}

function addStub(stubs) {
    const url = "http://localhost:2525/imposters/8090/stubs"
   stubs.forEach(stub => {
    fetch(url, {
       method: 'POST',
       headers: { 'Content-Type': 'application/json' },
       body: JSON.stringify({stub:stub})
   });
});
    
}
function createImposter() {
    const imposter = {
        port: 8090,
        protocol: 'http'
        
    }
    postImposter(imposter);
}


module.exports = { postImposter, createImposter, addStub };