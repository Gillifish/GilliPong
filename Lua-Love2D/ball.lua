Ball = {style = "", xPos = 0, yPos = 0, radius = 0, segments = 20, speed = 0, xVelocity = 0, yVelocity = 0}

function Ball:new (o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function Ball:reset(centerX, centerY)
    self.xVelocity = 0
    self.yVelocity = 0
    self.xPos = centerX
    self.yPos = centerY
end

function Ball:draw()
    love.graphics.circle(self.style, self.xPos, self.yPos, self.radius, self.segments)
end