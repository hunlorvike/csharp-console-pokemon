/*
Database: csharp_pokemons

CREATE TABLE Users (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(255),
    Password VARCHAR(255),
    CountHuntPokemon INT DEFAULT 30
);

CREATE TABLE Pokemons (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255),
    Type VARCHAR(255),
    PercentLevel FLOAT NOT NULL DEFAULT 0,
    Level INT,
    HP INT,
    Damage INT
);


CREATE TABLE UserPokemons (
    UserID INT,
    PokemonID INT,
    Count INT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(ID),
    FOREIGN KEY (PokemonID) REFERENCES Pokemons(ID),
    PRIMARY KEY (UserID, PokemonID)
);

INSERT INTO Pokemons (Name, Type, PercentLevel, Level, HP, Damage)
VALUES('Pikachu', 'Electric', 0, 1, 100, 20)

INSERT INTO Pokemons (Name, Type, PercentLevel, Level, HP, Damage)
VALUES
    ('Bulbasaur', 'Grass/Poison', 0, 1, 45, 49),
    ('Charmander', 'Fire', 0, 1, 39, 52),
    ('Squirtle', 'Water', 0, 1, 44, 48),
    ('Pikachu', 'Electric', 0, 1, 35, 55),
    ('Jigglypuff', 'Normal/Fairy', 0, 1, 115, 45),
    ('Mewtwo', 'Psychic', 0, 1, 106, 110),
    ('Eevee', 'Normal', 0, 1, 55, 55),
    ('Snorlax', 'Normal', 0, 1, 160, 110),
    ('Machop', 'Fighting', 0, 1, 70, 80),
    ('Pidgey', 'Normal/Flying', 0, 1, 40, 45),
    ('Geodude', 'Rock/Ground', 0, 1, 40, 80),
    ('Abra', 'Psychic', 0, 1, 25, 20),
    ('Kadabra', 'Psychic', 0, 1, 40, 35),
    ('Gastly', 'Ghost/Poison', 0, 1, 30, 35),
    ('Kangaskhan', 'Normal', 0, 1, 105, 95);


*/