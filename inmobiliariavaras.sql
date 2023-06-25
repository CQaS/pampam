-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 23-09-2021 a las 22:46:26
-- Versión del servidor: 10.4.17-MariaDB
-- Versión de PHP: 8.0.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariavaras`
--

DELIMITER $$
--
-- Funciones
--
CREATE DEFINER=`root`@`localhost` FUNCTION `Multa` (`m` INT) RETURNS INT(11) BEGIN 
DECLARE asignarMulta int default -1;

SELECT (( SELECT TIMESTAMPDIFF(MONTH, Curdate(), fecha_In) as Meses FROM contratos WHERE id_Cont = m) /(SELECT TIMESTAMPDIFF(MONTH, fecha_In, fecha_fin) as Meses FROM contratos WHERE id_Cont = m))
INTO asignarMulta
FROM contratos 
WHERE id_Cont = m;

IF asignarMulta > 0.5 THEN
SET asignarMulta = 1;
ELSEIF asignarMulta < 0.5 THEN
SET asignarMulta = 2;
END IF;
    
RETURN asignarMulta;

END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `numSigPago` (`ultimo` INT) RETURNS INT(11) BEGIN
    DECLARE ultimopago int default 1;
    
    SELECT IFNULL(max(num_Pago +1), 1)
    INTO ultimopago
    FROM pagos 
    WHERE contrato_Id = ultimo;
    
    RETURN ultimopago;
	
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id_Cont` int(10) NOT NULL,
  `fecha_In` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `valor` int(20) NOT NULL,
  `inm_Id` int(10) NOT NULL,
  `inq_Id` int(10) NOT NULL,
  `estado` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id_Cont`, `fecha_In`, `fecha_fin`, `valor`, `inm_Id`, `inq_Id`, `estado`) VALUES
(3, '2021-09-05', '2023-09-05', 66000, 9, 6, 1),
(4, '2021-08-20', '2026-08-20', 96000, 10, 7, 1),
(5, '2021-09-23', '2021-12-23', 86000, 18, 11, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id_Inm` int(5) NOT NULL,
  `dom_Inm` varchar(50) NOT NULL,
  `uso` varchar(20) NOT NULL,
  `tipo` varchar(20) NOT NULL,
  `ambientes` int(10) NOT NULL,
  `precio` int(20) NOT NULL,
  `prop_Id` int(5) NOT NULL,
  `imagen` varchar(300) NOT NULL,
  `estado` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id_Inm`, `dom_Inm`, `uso`, `tipo`, `ambientes`, `precio`, `prop_Id`, `imagen`, `estado`) VALUES
(9, 'Juan W Gez 324', 'Familiar', 'Hogar', 4, 76000, 4, '/img/Inmueble_CodProp_4\\Inmueble_16_09_202110_37_32.jpeg', 1),
(10, 'Marconi 1934', 'Familiar', 'Hogar', 5, 96000, 4, '/img/Inmueble_CodProp_4\\Inmueble_20_09_202106_36_21.jpg', 1),
(11, 'Av. Dos Venados 120', 'Familiar', 'Departamento', 4, 66000, 1, '/img/Inmueble_CodProp_1\\Inmueble_20_09_202107_48_32.jpg', 1),
(12, 'Av. Pte. Perón 1232', 'Familiar', 'Departamento', 6, 87000, 3, '/img/Inmueble_CodProp_3\\Inmueble_20_09_202107_55_30.jpg', 1),
(13, 'Olegario Andrade 543', 'Familiar', 'Hogar', 8, 306000, 5, '/img/Inmueble_CodProp_5\\Inmueble_20_09_202107_57_11.jpg', 1),
(14, 'Mitre 543', 'Familiar', 'Hogar', 8, 306000, 5, '/img/Inmueble_CodProp_5\\Inmueble_20_09_202107_57_11.jpg', 0),
(15, 'Colón 733', 'Comercial', 'Local', 5, 97000, 8, '/img/Inmueble_CodProp_8\\Inmueble_20_09_202107_59_12.jpg', 1),
(16, 'Salta 733', 'Familiar', 'Hogar', 5, 300000, 8, '/img/Inmueble_CodProp_8\\Inmueble_20_09_202108_03_16.jpg', 1),
(17, 'Paraguay 632	', 'Familiar', 'Departamento', 4, 76000, 7, '/img/Inmueble_CodProp_7\\Inmueble_20_09_202108_09_09.jpg', 1),
(18, 'Juan W Gez 334', 'Comercial', 'Local', 3, 86000, 10, '/img/Inmueble_CodProp_10\\Inmueble_23_09_202103_00_38.jpg', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id_Inq` int(10) NOT NULL,
  `dni_Inq` int(50) NOT NULL,
  `nombre_Inq` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `dom_Inq` varchar(50) NOT NULL,
  `tel_Inq` int(20) NOT NULL,
  `domicilio_Lab` varchar(50) NOT NULL,
  `nombre_Garante` varchar(50) NOT NULL,
  `dni_Garante` int(15) NOT NULL,
  `tel_Garante` int(20) NOT NULL,
  `estado` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id_Inq`, `dni_Inq`, `nombre_Inq`, `email`, `dom_Inq`, `tel_Inq`, `domicilio_Lab`, `nombre_Garante`, `dni_Garante`, `tel_Garante`, `estado`) VALUES
(2, 29876567, 'Vargas Lorena', 'vargas@gmail.com', 'Sucre 1232', 266532438, 'Maipu 123', 'Lopez Carlos', 26345445, 266455534, 0),
(3, 27343443, 'Gondolo Lucía', 'gondolo@gmail.com', 'Mitre 122', 0, 'San Martin 1827', 'Miranda Denise', 30567967, 0, 0),
(4, 29876567, 'Lorena Vargas', 'vargas@gmail.com', 'Sucre 1232', 0, 'San Martin 1827', 'Lopez Carlos', 26345445, 0, 0),
(5, 29876522, 'Gondolo Lucía', 'gondolo@gmail.com', 'Mitre 122', 0, 'Maipu 123', 'Miranda Denise', 26341145, 266431212, 0),
(6, 27343455, 'Marina Vargas', 'vargasmarina@gmail.com', 'San Martin 173', 265755661, 'Colón 111', 'Arguello Cristian', 26343454, 0, 1),
(7, 29876588, 'Andrade Hugo', 'andrade@gmail.com', 'Las Heras 1222', 0, 'Maipu 123', 'Morán Julia', 28345445, 0, 1),
(8, 27343465, 'Orquera Horacio', 'orquehoracio@gmail.com', 'Salta 236', 0, 'San Martin 1827', 'Pascual Pablo', 28345447, 265744482, 1),
(9, 27343656, 'Alaniz Patricia', 'patoalaniz@gmail.com', 'Maipú 732', 0, 'Colón 134', 'Paz Micaela', 28345432, 0, 1),
(10, 27343466, 'Lopez Araceli', 'aralo@gmail.com', 'Av. Illia 1232', 266467879, 'Colón 111', 'Foncueva Sergio', 28345499, 0, 1),
(11, 29378038, 'Ponce Paola', 'ponce@mail.com', 'San Martin 2000', 0, 'Maipu 2000', 'Arguello Cristian', 28345447, 0, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id_Pagos` int(10) NOT NULL,
  `num_Pago` int(50) NOT NULL,
  `fecha` date NOT NULL,
  `importe` decimal(15,0) NOT NULL,
  `contrato_Id` int(10) NOT NULL,
  `estado` int(15) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id_Pagos`, `num_Pago`, `fecha`, `importe`, `contrato_Id`, `estado`) VALUES
(1, 1, '2021-10-05', '66000', 3, 1),
(2, 1, '2021-09-20', '96000', 4, 1),
(3, 2, '2021-11-05', '66000', 3, 1),
(4, 2, '2021-10-20', '96000', 4, 1),
(5, 1, '2021-09-23', '86000', 5, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id_Prop` int(5) NOT NULL,
  `dni` int(20) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `dom_Prop` varchar(50) NOT NULL,
  `tel` int(20) NOT NULL,
  `estado` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_Prop`, `dni`, `nombre`, `dom_Prop`, `tel`, `estado`) VALUES
(1, 29685745, 'Toncón Cristian', 'San Martin 120', 266446473, 1),
(2, 28685745, 'Jofré Maria', 'Las Heras 123', 266456477, 0),
(3, 29785743, 'Acosta Pedro', 'Av. Lafinur 1232', 266456424, 1),
(4, 29681112, 'Funez Estefanía', 'Sucre 837', 266456411, 1),
(5, 27765765, 'Caravajal Celene', 'Mitre 543', 266455449, 1),
(6, 25685666, 'Leiva Mariana', 'Tomás Jofré 156', 266455886, 1),
(7, 30453454, 'Valdez Irma', 'Ituzaingo 632', 265744366, 1),
(8, 27223423, 'Lorenzo Tomás', 'Caseros 733', 265733499, 1),
(9, 23766555, 'Costa Natalia', 'Marconi 1927', 265733448, 1),
(10, 29939444, 'Costa Patricia', 'Marconi 1333', 26649834, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id_Us` int(10) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `avatar` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `contraseña` varchar(100) NOT NULL,
  `pregunta` varchar(200) NOT NULL,
  `rol` int(11) NOT NULL,
  `estado` int(15) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id_Us`, `nombre`, `apellido`, `avatar`, `email`, `contraseña`, `pregunta`, `rol`, `estado`) VALUES
(2, 'VarasAdministradora', 'Varas', '/img/Avatars/av1.png', 'varasadministradora@mail.com', 'KkKWc+Xi5HcpwLE0hONedm/8Rt48SCalxOXt9TEJRIA=', 'varasadministradora', 1, 1),
(3, 'Jorge', 'Perez', '/img/avatars\\avatar_.jpg', 'perez@mail.com', 'KkKWc+Xi5HcpwLE0hONedm/8Rt48SCalxOXt9TEJRIA=', 'jorge', 2, 1),
(5, 'Marcos', 'Santarelli', '/img/avatars\\avatar_marcossantarelli@mail.com.jpg', 'marcossantarelli@mail.com', 'o3jt8YZ9xYzbUmta4lycVhd0PMcjDUv8etHr66HVyM0=', 'santarelli', 1, 1),
(6, 'Luciana', 'Funez', '/img/avatars\\avatar_lufunez@mail.com.jpg', 'lufunez@mail.com', 'N4J8QS55Itx14POhJ5C6jVJvwF4PVZ9nQUM/bOd9KjI=', 'funez', 2, 1),
(8, 'Juan', 'Vallica', '/img/avatars\\avatar_8.jpg', 'vallicaadministrador@mail.com', '9c97mQ7Q75KuHGN9cp1Hh9rGI6UvCL1Q1aDiWZ574z8=', 'vallica', 1, 1),
(9, 'Jorge', 'Lanuz', '/img/avatars\\avatar_9.jpg', 'lanuz@mail.com', 'lal+dWEVxqA7hpOKze2u5GEDXMY5t0xtiMqcyIQ+P4I=', 'lanuz', 2, 0),
(10, 'hhh', 'ggg', '/img/avatars\\avatar_10.jpg', 'gondolo@gmail.com', 'X8IXTnAq7arL+3FQaLbcTjb2EAMSmBEUd0LyZVkJIEk=', 'mmmm', 2, 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id_Cont`),
  ADD KEY `inm_Id` (`inm_Id`),
  ADD KEY `inq_Id` (`inq_Id`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id_Inm`),
  ADD KEY `prop_Id` (`prop_Id`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id_Inq`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id_Pagos`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_Prop`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_Us`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id_Cont` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id_Inm` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id_Inq` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id_Pagos` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_Prop` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_Us` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`inq_Id`) REFERENCES `inquilinos` (`id_Inq`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`inm_Id`) REFERENCES `inmuebles` (`id_Inm`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`prop_Id`) REFERENCES `propietarios` (`id_Prop`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
