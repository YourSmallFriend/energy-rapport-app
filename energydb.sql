-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 28, 2024 at 02:55 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `energydb`
--

-- --------------------------------------------------------

--
-- Table structure for table `gasverbruik`
--

CREATE TABLE `gasverbruik` (
  `id` int(11) NOT NULL,
  `gebruiker_id` int(11) NOT NULL,
  `opnamedatum` date NOT NULL,
  `gas_stand` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `gebruikers`
--

CREATE TABLE `gebruikers` (
  `id` int(11) NOT NULL,
  `naam` varchar(100) NOT NULL,
  `gebruikersnaam` varchar(128) NOT NULL,
  `email` varchar(255) NOT NULL,
  `wachtwoord_hash` varchar(255) NOT NULL,
  `aanmaakdatum` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `gebruikers`
--

INSERT INTO `gebruikers` (`id`, `naam`, `gebruikersnaam`, `email`, `wachtwoord_hash`, `aanmaakdatum`) VALUES
(9, 'Samuel', 'samuel442', '400042808@st.roc.a12.nl', 'ToDSf8E3nNw56v7u1kfZqrsYgE3x2xI9fOuS+EKPyyI=', '2024-11-28 13:34:34'),
(10, 'Marco', 'Marcito', 'Marckisd122@dickmail.org', 'dwzcQckxI8vc8ek6nbZ0YFYLe4i8BC6hmjQuFnQYVQ8=', '2024-11-28 13:52:29');

-- --------------------------------------------------------

--
-- Table structure for table `stroomverbruik`
--

CREATE TABLE `stroomverbruik` (
  `id` int(11) NOT NULL,
  `gebruiker_id` int(11) NOT NULL,
  `opnamedatum` date NOT NULL,
  `stand_normaal` int(11) NOT NULL,
  `stand_dal` int(11) NOT NULL,
  `teruglevering_normaal` int(11) NOT NULL,
  `teruglevering_dal` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `gasverbruik`
--
ALTER TABLE `gasverbruik`
  ADD PRIMARY KEY (`id`),
  ADD KEY `gebruiker_id` (`gebruiker_id`);

--
-- Indexes for table `gebruikers`
--
ALTER TABLE `gebruikers`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indexes for table `stroomverbruik`
--
ALTER TABLE `stroomverbruik`
  ADD PRIMARY KEY (`id`),
  ADD KEY `gebruiker_id` (`gebruiker_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `gasverbruik`
--
ALTER TABLE `gasverbruik`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `gebruikers`
--
ALTER TABLE `gebruikers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `stroomverbruik`
--
ALTER TABLE `stroomverbruik`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `gasverbruik`
--
ALTER TABLE `gasverbruik`
  ADD CONSTRAINT `gasverbruik_ibfk_1` FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruikers` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `stroomverbruik`
--
ALTER TABLE `stroomverbruik`
  ADD CONSTRAINT `stroomverbruik_ibfk_1` FOREIGN KEY (`gebruiker_id`) REFERENCES `gebruikers` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
