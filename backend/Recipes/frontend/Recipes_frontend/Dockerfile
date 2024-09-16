FROM node:18

WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

COPY /package.json ./
COPY /package-lock.json ./
RUN npm install --unsafe-perm node-sass

COPY . ./

CMD ["npm", "run dev"]