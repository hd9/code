/// <reference path="../../lib/jasmine/jasmine.js"/>
/// <reference path="../mathhelper.js"/>

describe("MathHelper", function () {
    it("should add 2 numbers", function () {
        var mh = MathHelper;
        expect(mh.add(1, 2)).toBe(3);
    });
});

