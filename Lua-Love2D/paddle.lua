Paddle = {style = "", xPos = 0, yPos = 0, width = 0, height = 0}

function Paddle:new (o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function Paddle:moveUp(v)
    self.yPos = self.yPos - v
end

function Paddle:moveDown(v)
    self.yPos = self.yPos + v
end

function Paddle:draw()
    love.graphics.rectangle(self.style, self.xPos, self.yPos, self.width, self.height)
end