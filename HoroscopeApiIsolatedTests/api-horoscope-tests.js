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
        createImposter();
        parser.dereference('./swagger.json', function (err, api) {
            if (err) return done(err)
            dereferencedSwagger = api
            done()
        })
    })

    describe('Horoscope API Through Zodiac', function() {
        it('Fetch Horoscope For Today Successfully', function (done) {
            var zodiac = 'libra';
            addValidService(zodiac,'today');
            const token= generateToken();
            const todayDate = new Date();
            const formattedTodayDate= formatDateWithoutLeadingZeroes(todayDate);
            hippie(dereferencedSwagger, hippieOptions)
                .base(`${baseUrl}`)
                .header("User-Agent", "hippie")
                .header("authorization",`bearer ${token}`)
                .json()
                .get('/api/Horoscope/{zodiac}')
                .qs({ day: 'today' })
                .pathParams({
                    zodiac: `${zodiac}`,
                  })
                .end(function (err, res, body) {
                    if (err) return done(err);
                    expect(res.statusCode).to.equal(200);
                    expect(body.sun_sign).to.equal(zodiac);
                    expect(body.prediction_date).to.equal('6-2-2024');
                    done();
                });

        });

        it('Fetch Horoscope For Tomorrow Successfully', function (done) {
            var zodiac = 'libra';
            addValidService(zodiac,'tomorrow');
           const token= generateToken();
           const todayDate = new Date();
           const tomorrowDate = new Date();
           tomorrowDate.setDate(todayDate.getDate()+1);
           const formattedTomorrowDate= formatDateWithoutLeadingZeroes(tomorrowDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/{zodiac}')
               .qs({ day: 'tomorrow' })
               .pathParams({
                   zodiac: 'libra',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.sun_sign).to.equal("libra");
                   expect(body.prediction_date).to.equal('6-2-2024');
                   done();
               });

        });

        it('Fetch Horoscope For Yesterday Successfully', function (done) {
            var zodiac = 'libra';
            addValidService(zodiac,'yesterday');
           const token= generateToken();
           const todayDate = new Date();
           const yesterdayDate = new Date();
           yesterdayDate.setDate(todayDate.getDate()-1) ;
           const formattedYesterdayDate= formatDateWithoutLeadingZeroes(yesterdayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/{zodiac}')
               .qs({ day: 'yesterday' })
               .pathParams({
                   zodiac: 'libra',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.sun_sign).to.equal("libra");
                   expect(body.prediction_date).to.equal('6-2-2024');
                   done();
               });

        });

        it('should return not found if zodiac is empty', function (done) {
           const token= generateToken();
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/{zodiac}')
               .qs({ day: 'today' })
               .pathParams({
                   zodiac: '',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(404);
                   done();
               });

        });

        it('should return internal server error if zodiac is any random string', function (done) {
            addInvalidService('asdg');
           const token= generateToken();
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/{zodiac}')
               .qs({ day: 'today' })
               .pathParams({
                   zodiac: 'asdg',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(500);
                   done();
               });

        });

        it('should return bad request if day is null', function (done) {
           const token= generateToken();
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/{zodiac}')
               .pathParams({
                   zodiac: 'aries',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(400);
                   done();
               });

        });

        it('should return zodiac for yesterday if day is empty or any random string', function (done) {
        addValidService('libra','tdas');
           const token= generateToken();
           const todayDate = new Date();
           const yesterdayDate = new Date();
           yesterdayDate.setDate(todayDate.getDate()-1) ;
           const formattedYesterdayDate= formatDateWithoutLeadingZeroes(yesterdayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/{zodiac}')
               .qs({ day: 'tdas' })
               .pathParams({
                   zodiac: 'libra',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.prediction_date).to.equal("6-2-2024");
                   done();
               });

        });

        it('should return unauthorized if token is not sent', function (done) {         
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .json()
               .get('/api/Horoscope/{zodiac}')
               .qs({ day: 'today' })
               .pathParams({
                   zodiac: 'libra',
                 })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(401);
                   done();
               });

        });

        

        
     
    });

  describe('Horoscope API Through DateOfBirth', function() {
       it('Fetch Horoscope For Today Successfully', function (done) {
           addValidService('libra','today');
           const token= generateToken();
           const todayDate = new Date();
           const formattedTodayDate= formatDateWithoutLeadingZeroes(todayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "09/25/2001", 
                   day: 'today' })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.sun_sign).to.equal("libra");
                   expect(body.prediction_date).to.equal("6-2-2024");
                   done();

               });

       });

        
       it('Fetch Horoscope For Tomorrow Successfully', function (done) {
           addValidService('libra','tomorrow');
           const token= generateToken();
           const todayDate = new Date();
           const tomorrowDate = new Date();
           tomorrowDate.setDate(todayDate.getDate()+1);
           const formattedTomorrowDate= formatDateWithoutLeadingZeroes(tomorrowDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "09/25/2001", 
                   day: 'tomorrow' })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.sun_sign).to.equal("libra");
                   expect(body.prediction_date).to.equal("6-2-2024");
                  done();

               });

       });

        
       it('Fetch Horoscope For Yesterday Successfully', function (done) {
        addValidService('libra','yesterday');
           const token= generateToken();
           const todayDate = new Date();
           const yesterdayDate = new Date();
           yesterdayDate.setDate(todayDate.getDate()-1) ;
           const formattedYesterdayDate= formatDateWithoutLeadingZeroes(yesterdayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "09/25/2001", 
                   day: 'yesterday' })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.sun_sign).to.equal("libra");
                   expect(body.prediction_date).to.equal("6-2-2024");
                  done()

               });

       });

       it('should return bad request with message if either day or month is inavlid in date of birth', function (done) {
           const token= generateToken();
           const todayDate = new Date();
           const formattedTodayDate= formatDateWithoutLeadingZeroes(todayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "13/36/2001", 
                   day: 'today' })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(400);
                   expect(body.errors.dateOfBirth[0]).to.equal("The value '13/36/2001' is not valid.");
                   done();

               });

       });

       it('should return bad request with message if day field is null', function (done) {
           const token= generateToken();
           const todayDate = new Date();
           const formattedTodayDate= formatDateWithoutLeadingZeroes(todayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "13/36/2001", 
                    })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(400);
                   expect(body.errors.day[0]).to.equal("The day field is required.");
                   done();

               });

       });

       it('should return zodaic for yesterday if day is any random string or empty', function (done) {
        addValidService('libra','yesterday');
           const token= generateToken();
           const todayDate = new Date();
           const yesterdayDate = new Date();
           yesterdayDate.setDate(todayDate.getDate()-1) ;
           const formattedYesterdayDate= formatDateWithoutLeadingZeroes(yesterdayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .header("authorization",`bearer ${token}`)
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "09/25/2001", 
                   day: 'asdsd' })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(200);
                   expect(body.prediction_date).to.equal("6-2-2024");
                   done();

               });

       });

       it('should return unauthorised if token is not sent', function (done) {
           const todayDate = new Date();
           const formattedTodayDate= formatDateWithoutLeadingZeroes(todayDate);
           hippie(dereferencedSwagger, hippieOptions)
               .base(`${baseUrl}`)
               .header("User-Agent", "hippie")
               .json()
               .get('/api/Horoscope/dob')
               .qs({dateOfBirth: "12/12/2001", 
                   day: 'today' })
               .end(function (err, res, body) {
                   if (err) return done(err);
                   expect(res.statusCode).to.equal(401);
                   done();

               });

       });



  });

});