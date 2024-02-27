const chai = require('chai');
const expect = chai.expect
const SwaggerParser = require('swagger-parser')
var parser = new SwaggerParser()
const hippie = require('hippie-swagger');
const { generateToken } = require('./tokenHelper');
const { formatDateWithoutLeadingZeroes } = require('./dateHelper');
const { addValidService, addInvalidService } = require('./horoscope-service');
const { createImposter } = require('./mountebank-helper');
let baseUrl = 'http://localhost:5000'
var dereferencedSwagger


let hippieOptions = {
    validateResponseSchema: false,
    validateParameterSchema: false,
    errorOnExtraParameters: false,
    errorOnExtraHeaderParameters: false
};

describe('Test of', function () {
    this.timeout(20000)

    before(function (done) {
        parser.dereference('./swagger.json', function (err, api) {
            if (err) return done(err)
            dereferencedSwagger = api
            done()
        })
    })



    describe('Login API', function() {
        it('should authenticate a user with correct credentials', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Login')
                .send({
                    email: 'Afeef@gmail.com',
                    password: 'Afeef@123'
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(200);
                    done();

                });

        });

        it('should print unauthroized due to incorrect credentials', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Login')
                .send({
                    email: 'Rahul@gmail.com',
                    password: 'Rahul@123'
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(401);
                    done();
                });

        });

        it('should return bad request with validation messages', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Login')
                .send({
                    email: '',
                    password: ''
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(400);
                    expect(body.errors.Email[0]).to.equal("'Email' must not be empty.");
                    expect(body.errors.Email[1]).to.equal("'Email' is not a valid email address.");
                    expect(body.errors.Password[0]).to.equal("'Password' must not be empty.");                    
                    done();
                });

        });
    });

    describe('Register API', function() {
        it('should create a new user successfully', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Register')
                .send({
                    email : 'Virat@gmail.com',
                    Name : 'Virat',
                    password : 'Virat@123'
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(201);
                      done()
                });

        });

        it('should display conflict if registering with already existing email', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Register')
                .send({
                    email : 'Afeef@gmail.com',
                    Name : 'Afeef',
                    password : 'Afeef@123'
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(409);
                    done()
                });

        });

        it('should return bad request if fields are empty', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Register')
                .send({
                    email : '',
                    Name : '',
                    password : ''
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(400);
                    expect(body.errors.Name[0]).to.equal("'Name' must not be empty.");
                    expect(body.errors.Email[0]).to.equal("'Email' must not be empty.");
                    expect(body.errors.Email[1]).to.equal("'Email' is not a valid email address.");
                    expect(body.errors.Password[0]).to.equal("'Password' must not be empty.");
                    done()
                });

        });

        it('should return bad request if Name or password format is wrong', function (done) {
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .json()
                .post('/api/User/Register')
                .send({
                    email : 'Afe@gmal.com',
                    Name : 'Af123',
                    password : 'sa'
                })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(400);
                    expect(body.errors.Name[0]).to.equal("Name can only contain alphabets, space or apostrophe");
                    expect(body.errors.Password[0]).to.equal("Password must contain atleast one upper case, one lower case, one number, one @ symbol with a length of atleast 8");
                    done()
                });

        });


    });

 describe('User History API', function() {
   it('should provide searched zodiac', function (done) {
       const token= generateToken();
       hippie(dereferencedSwagger, hippieOptions)
       .base(`${baseUrl}`)
           .header("User-Agent", "hippie")
           .header("Authorization", `Bearer ${token}`)
           .json()
           .get('/api/User/History')
           .end(function (err, res, body) {
               if (err) return done(err);
               expect(res.statusCode).to.equal(200);
               expect(body.length).to.equal(2);
               done();
           });
 
   });

   it('should return unauthorized if token is not passed', function (done) {
    hippie(dereferencedSwagger, hippieOptions)
    .base(`${baseUrl}`)
        .header("User-Agent", "hippie")
        .json()
        .get('/api/User/History')
        .end(function (err, res, body) {
            if (err) return done(err);
            expect(res.statusCode).to.equal(401);
            done();
        });

});
 }); 



});

