CREATE DATABASE PlotMusic;
GO

USE PlotMusic;
GO

-- PERFIL
CREATE TABLE Perfil (
    IdPerfil INT IDENTITY PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL
);
GO

-- USUARIO 
CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    IdPerfil INT NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Senha VARCHAR(100) NOT NULL,
    DataCriacao DATE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_UsuarioPerfil FOREIGN KEY (IdPerfil) REFERENCES Perfil(IdPerfil)
);
GO

-- ARTISTA
CREATE TABLE Artista (
    IdArtista INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    ImagemURL VARCHAR(255)
);
GO

-- ALBUM 
CREATE TABLE Album (
    IdAlbum INT IDENTITY(1,1) PRIMARY KEY,
    IdArtista INT NOT NULL,
    Titulo VARCHAR(100),
    AnoLancamento INT NOT NULL,
    ImagemURL VARCHAR(255),

    CONSTRAINT fk_AlbumArtista FOREIGN KEY (IdArtista) REFERENCES Artista(IdArtista) ON DELETE CASCADE
);
GO

-- GENERO
CREATE TABLE Genero (
    IdGenero INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100),
    Descricao TEXT 
);
GO

-- MUSICA
CREATE TABLE Musica (
    IdMusica INT IDENTITY(1,1) PRIMARY KEY,
    IdAlbum INT NOT NULL,
    IdArtista INT NOT NULL,
    IdGenero INT NOT NULL,
    Titulo VARCHAR(100),
    Duracao TIME NOT NULL,

    CONSTRAINT fk_MusicaAlbum FOREIGN KEY (IdAlbum) 
        REFERENCES Album(IdAlbum) ON DELETE CASCADE,
        
    CONSTRAINT fk_MusicaArtista FOREIGN KEY (IdArtista) 
        REFERENCES Artista(IdArtista) ON DELETE NO ACTION, 
        
    CONSTRAINT fk_MusicaGenero FOREIGN KEY (IdGenero) 
        REFERENCES Genero(IdGenero) ON DELETE NO ACTION 
);
GO

-- CARGA DE DADOS INICIAIS (SEED)

-- Perfis 
INSERT INTO Perfil (Nome) VALUES ('Administrador'), ('Comum');

-- Usuário Administrador (senha com hash BCrypt para 'Admin@123')
INSERT INTO Usuario (Nome, Email, Senha, IdPerfil) 
VALUES ('Admin Plot', 'admin@plotmusic.com', '$2a$10$N.zmdr9k7uOCQb376NoUnuTJ8iAt6Z5EHsM8lE9lBOsl7iKTVKIUi', 1);

-- Gêneros 
INSERT INTO Genero (Nome, Descricao) VALUES 
('Rock', 'Estilo musical com guitarras marcantes e ritmo forte.'),
('MPB', 'Musica Popular Brasileira.');

-- Artista 
INSERT INTO Artista (Nome, ImagemURL) VALUES 
('Titãs', 'https://admin.cnnbrasil.com.br/wp-content/uploads/sites/12/2022/11/Copia-de-BW-20221024-Titas_Grupo_0524.jpg?w=1200&h=1200&crop=1');

-- Álbum 
INSERT INTO Album (Titulo, AnoLancamento, IdArtista, ImagemURL) VALUES 
('Cabeça Dinossauro', 1986, 1, 'https://upload.wikimedia.org/wikipedia/pt/6/6f/Tit%C3%A3s_1984.jpg');

-- Músicas 
INSERT INTO Musica (Titulo, Duracao, IdAlbum, IdArtista, IdGenero) VALUES 
('Polícia', '00:02:07', 1, 1, 1),
('Homem Primata', '00:03:27', 1, 1, 1);
GO
