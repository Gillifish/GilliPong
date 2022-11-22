require "paddle"
require "ball"

love.window.setTitle("Pong")
gameWidth = love.graphics.getWidth()
gameHeight = love.graphics.getHeight()
paddleVelocity = 2

player1Score = 0
player2Score = 0
roundStart = false

p1 = Paddle:new{style = "line", xPos = 30, yPos = (gameHeight / 2) - 40, width = 20, height = 80}
p2 = Paddle:new{style = "line", xPos = gameWidth - 50, yPos = (gameHeight / 2) - 40, width = 20, height = 80}
ball = Ball:new{style = "fill", xPos = gameWidth / 2, yPos =  gameHeight / 2, radius = 5, segments = 200, speed = 3, xVelocity = 0, yVelocity = 0}
ball.yV = 0

function love.load()
    font = love.graphics.newFont(32)
end

function love.draw()
    drawHUD()
    p1:draw()
    p2:draw()
    ball:draw()
end

function love.update()
    if ball.xVelocity == 0 and ball.yVelocity == 0 and love.keyboard.isDown("space") and roundStart == false then
        ball.xVelocity = -3
        ball.yVelocity = 0
        roundStart = true
    end

    player1Input(paddleVelocity)
    player2Input(paddleVelocity)

    scoreCheck(ball)
    checkBallBounds(ball)

    if ballCollision(p1) then
        ball.yVelocity = ball.speed * math.sin(math.deg(getAngle(ball, p1)))
        ball.xVelocity = -1 * ball.xVelocity
        
    elseif ballCollision(p2) then
        ball.yVelocity = ball.speed * math.sin(math.deg(getAngle(ball, p2)))
        ball.xVelocity = -1 * ball.xVelocity
    end

    ball.xPos = ball.xPos + ball.xVelocity
    ball.yPos = ball.yPos + ball.yVelocity
end

function drawHUD()
    love.graphics.setFont(font)
    love.graphics.line(gameWidth / 2, 0, gameWidth / 2, gameHeight)
    love.graphics.print(player1Score, (gameWidth / 2) - 50, 20)
    love.graphics.print(player2Score, (gameWidth / 2) + 25, 20)
end

function player1Input(v)
    if love.keyboard.isDown("s") then
        p1:moveDown(v)
        checkBounds(p1)
    elseif love.keyboard.isDown("w") then
        p1:moveUp(v)
        checkBounds(p1)
    end
end

function player2Input(v)
    if love.keyboard.isDown("down") then
        p2:moveDown(v)

        checkBounds(p2)
    elseif love.keyboard.isDown("up") then
        p2:moveUp(v)
        checkBounds(p2)
    end
end

function checkBounds(paddle)
    if paddle.yPos > gameHeight - paddle.height then
        paddle.yPos = gameHeight - paddle.height
    elseif paddle.yPos < 0 then
        paddle.yPos = 0
    end
end

function checkBallBounds(b)
    if b.yPos > gameHeight then
        b.yVelocity = b.yVelocity * -1
    elseif b.yPos < 0 then
        b.yVelocity = b.yVelocity * -1
    end
end

function ballCollision(paddle)
    if (ball.xPos + ball.radius > paddle.xPos and ball.xPos - ball.radius < paddle.xPos + paddle.width) and (ball.yPos > paddle.yPos and ball.yPos < paddle.yPos + paddle.height) then
        return true
    else
        return false
    end
end

function scoreCheck(ball)
    if ball.xPos < 0 then 
        player2Score = player2Score + 1
        ball:reset(gameWidth / 2, gameHeight / 2)

        roundStart = false

        return true
    elseif ball.xPos > gameWidth then
        player1Score = player1Score + 1
        ball:reset(gameWidth / 2, gameHeight / 2)

        roundStart = false

        return true
    else 
        return false
    end
end

function getAngle(ball, paddle)
    median = paddle.height / 2
    ballPos = ball.yPos - paddle.yPos

    if ballPos < median and ball.xVelocity < 0 then
        print("Top Left")
        angle = (ballPos / median) * 90
        print(angle)
        return angle + 90
    elseif ballPos < median and ball.xVelocity > 0 then
        print("Top Right")
        print(angle)
        angle = (ballPos / median) * 90
        return angle + 90
    elseif ballPos > median and ball.xVelocity < 0 then
        print("Bottom Left")
        angle = ((ballPos - median) / median) * 90
        print(angle)
        return angle
    elseif ballPos > median and ball.xVelocity > 0 then
        print("Bottom Right")
        angle = ((ballPos - median) / median) * 90
        print(angle)
        return angle + 90
    else
        return 0
    end
end